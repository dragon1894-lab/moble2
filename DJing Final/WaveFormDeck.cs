using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace DJing
{
    /// <summary>
    /// 개별 덱의 오디오 재생, BPM 변환, 파형 렌더링을 담당하는 클래스
    /// </summary>
    public class WaveformDeck : IDisposable
    {
        private readonly PictureBox targetPictureBox;
        private readonly MasterAudioEngine masterAudio;
        private AudioFileReader? audioFile;
        private SpeedSampleProvider? speedProvider;
        private DeckEqualizerSampleProvider? equalizerProvider;
        private SoundTouchSampleProvider? pitchProvider;
        private LiveWaveformSampleProvider? liveWaveformProvider;
        private PlaybackGateSampleProvider? playbackGate;
        private float[]? peaks;
        private readonly System.Windows.Forms.Timer renderTimer;
        public TimeSpan CurrentTime => audioFile?.CurrentTime ?? TimeSpan.Zero;
        public TimeSpan TotalTime => audioFile?.TotalTime ?? TimeSpan.Zero;
        public event Action? PositionChanged;
        public GlitchStutterProvider? GlitchProvider { get; private set; }

        public AudioFileReader? AudioFile => audioFile;
        public bool HasAudio => audioFile != null && peaks != null && peaks.Length > 0;
        public bool IsPlaying => playbackGate?.IsPlaying == true;

        public float BaseBpm { get; set; } = 120.0f;
        private float midGainDb;
        private float bassValue;
        private float filterValue;
        private float pitchSemitones;
        private float targetBpm = 120f;
        private bool autoLoopActive;
        private float autoLoopBeats = 4f;
        private TimeSpan autoLoopStart;
        private TimeSpan autoLoopEnd;

        public bool IsAutoLoopActive => autoLoopActive;

        public WaveformDeck(PictureBox pictureBox, MasterAudioEngine masterAudio)
        {
            targetPictureBox = pictureBox;
            this.masterAudio = masterAudio ?? throw new ArgumentNullException(nameof(masterAudio));

            renderTimer = new System.Windows.Forms.Timer();
            renderTimer.Interval = 30; // 약 33 FPS 파형 애니메이션
            renderTimer.Tick += (s, e) =>
            {
                UpdateAutoLoop();
                targetPictureBox.Invalidate();
                PositionChanged?.Invoke();
            };

            targetPictureBox.Paint += OnPaint;
            targetPictureBox.MouseDown += OnMouseDown;
            targetPictureBox.Resize += (s, e) => targetPictureBox.Invalidate();
        }

        public void SetVolume(float volume)
        {
            if (audioFile != null)
            {
                audioFile.Volume = Math.Clamp(volume, 0.0f, 1.0f);
            }
        }

        public void SetTargetBpm(float targetBpm)
        {
            this.targetBpm = Math.Max(1f, targetBpm);
            if (speedProvider != null && BaseBpm > 0)
            {
                speedProvider.PlaybackRate = this.targetBpm / BaseBpm;
            }
            if (autoLoopActive) RecalculateAutoLoopEnd();
        }

        public bool ToggleAutoLoop(float beats)
        {
            if (audioFile == null) return false;

            autoLoopBeats = Math.Max(0.125f, beats);
            autoLoopActive = !autoLoopActive;
            if (autoLoopActive)
            {
                autoLoopStart = audioFile.CurrentTime;
                RecalculateAutoLoopEnd();
            }
            return autoLoopActive;
        }

        public void SetAutoLoopBeats(float beats)
        {
            autoLoopBeats = Math.Max(0.125f, beats);
            if (autoLoopActive) RecalculateAutoLoopEnd();
        }

        public void SetMidGain(float gainDb)
        {
            midGainDb = Math.Clamp(gainDb, -30f, 12f);
            equalizerProvider?.SetMidGain(midGainDb);
        }

        public void SetBass(float value)
        {
            bassValue = Math.Clamp(value, -1f, 1f);
            equalizerProvider?.SetBass(bassValue);
        }

        public void SetFilter(float value)
        {
            filterValue = Math.Clamp(value, -1f, 1f);
            equalizerProvider?.SetFilter(filterValue);
        }

        public void SetPitch(float semitones)
        {
            pitchSemitones = Math.Clamp(semitones, -20f, 20f);
            pitchProvider?.SetPitchSemitones(pitchSemitones);
        }

        public void LoadAudio(string filePath, float baseBpm = 120.0f)
        {
            CleanupAudio();
            this.BaseBpm = baseBpm;
            targetBpm = baseBpm > 0f ? baseBpm : 120f;

            if (!System.IO.File.Exists(filePath)) return;

            // 1. [파형 분석] Peak 데이터 추출
            using (var tempReader = new AudioFileReader(filePath))
            {
                int width = Math.Max(1, targetPictureBox.Width);
                peaks = new float[width];

                long totalSamples = tempReader.Length / sizeof(float);
                int samplesPerPixel = (int)(totalSamples / width);
                if (samplesPerPixel < 1) samplesPerPixel = 1;

                float[] buffer = new float[samplesPerPixel];

                for (int i = 0; i < width; i++)
                {
                    int read = tempReader.Read(buffer, 0, samplesPerPixel);
                    if (read == 0) break;

                    float max = 0f;
                    for (int j = 0; j < read; j++)
                    {
                        float absVal = Math.Abs(buffer[j]);
                        if (absVal > max) max = absVal;
                    }
                    peaks[i] = max;
                }
            }

            // 2. 덱별 오디오 파이프라인:
            // Reader -> Speed -> Bass/Mid/Filter -> Pitch -> Effects
            audioFile = new AudioFileReader(filePath);
            speedProvider = new SpeedSampleProvider(audioFile);
            equalizerProvider = new DeckEqualizerSampleProvider(
                speedProvider,
                midGainDb,
                bassValue,
                filterValue);
            pitchProvider = new SoundTouchSampleProvider(equalizerProvider, pitchSemitones);
            GlitchProvider = new GlitchStutterProvider(pitchProvider);
            ISampleProvider masterFormatProvider = ConvertToMasterFormat(GlitchProvider);
            liveWaveformProvider = new LiveWaveformSampleProvider(masterFormatProvider);
            playbackGate = new PlaybackGateSampleProvider(liveWaveformProvider);
            masterAudio.AddInput(playbackGate);

            targetPictureBox.Invalidate();
        }

        public void Play()
        {
            if (playbackGate == null || audioFile == null) return;

            try
            {
                if (audioFile.Position >= audioFile.Length)
                {
                    audioFile.Position = 0;
                    pitchProvider?.Reset();
                }

                if (!playbackGate.IsPlaying)
                {
                    playbackGate.IsPlaying = true;
                    renderTimer.Start();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Audio Play Error] {ex.Message}");
                renderTimer.Stop();
            }
        }

        public void Pause()
        {
            if (playbackGate?.IsPlaying == true)
            {
                playbackGate.IsPlaying = false;
                renderTimer.Stop();
                targetPictureBox.Invalidate();
            }
        }

        public void Stop()
        {
            if (playbackGate != null && audioFile != null)
            {
                playbackGate.IsPlaying = false;
                audioFile.CurrentTime = TimeSpan.Zero;
                pitchProvider?.Reset();
                autoLoopActive = false;
                renderTimer.Stop();
                targetPictureBox.Invalidate();
                PositionChanged?.Invoke();
            }
        }

        private void OnPaint(object? sender, PaintEventArgs e)
        {
            DrawWaveform(
                e.Graphics,
                targetPictureBox.ClientRectangle,
                Color.FromArgb(255, 120, 0),
                Color.FromArgb(80, 85, 100),
                true);
        }

        public void DrawWaveform(
            Graphics graphics,
            Rectangle bounds,
            Color playedColor,
            Color remainingColor,
            bool clearBackground = false)
        {
            if (clearBackground)
            {
                using var background = new SolidBrush(Color.FromArgb(20, 22, 32));
                graphics.FillRectangle(background, bounds);
            }
            if (peaks == null || audioFile == null || peaks.Length == 0 || bounds.Width <= 0 || bounds.Height <= 0) return;

            int currentWidth = bounds.Width;
            int currentHeight = bounds.Height;

            double progress = 0;
            if (audioFile.TotalTime.TotalMilliseconds > 0)
            {
                progress = audioFile.CurrentTime.TotalMilliseconds / audioFile.TotalTime.TotalMilliseconds;
            }
            int progressX = bounds.Left + (int)(currentWidth * progress);

            float stepX = (float)currentWidth / peaks.Length;
            float penWidth = Math.Max(1.0f, stepX);

            for (int i = 0; i < peaks.Length; i++)
            {
                float x = bounds.Left + i * stepX;
                int barHeight = (int)(peaks[i] * (currentHeight * 0.8f));

                Color barColor = x < progressX ? playedColor : remainingColor;

                using (Pen pen = new Pen(barColor, penWidth))
                {
                    graphics.DrawLine(pen, x, bounds.Bottom, x, bounds.Bottom - barHeight);
                }
            }

            using (Pen whitePen = new Pen(Color.White, 2))
            {
                graphics.DrawLine(whitePen, progressX, bounds.Top, progressX, bounds.Bottom);
            }
        }

        private void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (audioFile == null || audioFile.TotalTime.TotalMilliseconds <= 0) return;

            SeekByRatio((double)e.X / targetPictureBox.Width);
        }

        public void SeekByRatio(double targetRatio)
        {
            if (audioFile == null || audioFile.TotalTime.TotalMilliseconds <= 0) return;
            targetRatio = Math.Clamp(targetRatio, 0d, 1d);
            audioFile.CurrentTime = TimeSpan.FromMilliseconds(audioFile.TotalTime.TotalMilliseconds * targetRatio);
            pitchProvider?.Reset();
            if (autoLoopActive)
            {
                autoLoopStart = audioFile.CurrentTime;
                RecalculateAutoLoopEnd();
            }
            targetPictureBox.Invalidate();
            PositionChanged?.Invoke();
        }

        public int CopyLiveWaveform(float[] destination)
        {
            if (liveWaveformProvider == null)
            {
                Array.Clear(destination, 0, destination.Length);
                return 0;
            }
            Array.Clear(destination, 0, destination.Length);
            return liveWaveformProvider.CopyRecentSamples(destination);
        }

        private void UpdateAutoLoop()
        {
            if (!autoLoopActive || audioFile == null || audioFile.CurrentTime < autoLoopEnd) return;
            audioFile.CurrentTime = autoLoopStart;
            pitchProvider?.Reset();
        }

        private void RecalculateAutoLoopEnd()
        {
            if (audioFile == null) return;
            double seconds = autoLoopBeats * (60d / Math.Max(1f, targetBpm));
            autoLoopEnd = autoLoopStart + TimeSpan.FromSeconds(seconds);
            if (autoLoopEnd > audioFile.TotalTime) autoLoopEnd = audioFile.TotalTime;
        }

        private static ISampleProvider ConvertToMasterFormat(ISampleProvider source)
        {
            ISampleProvider result = source;
            if (result.WaveFormat.Channels == 1)
                result = new MonoToStereoSampleProvider(result);
            else if (result.WaveFormat.Channels != MasterAudioEngine.Channels)
                throw new NotSupportedException("현재 녹음 믹서는 모노 또는 스테레오 음원만 지원합니다.");

            if (result.WaveFormat.SampleRate != MasterAudioEngine.SampleRate)
                result = new WdlResamplingSampleProvider(result, MasterAudioEngine.SampleRate);
            return result;
        }

        public void CleanupAudio()
        {
            renderTimer.Stop();
            autoLoopActive = false;

            if (playbackGate != null)
            {
                playbackGate.IsPlaying = false;
                masterAudio.RemoveInput(playbackGate);
                playbackGate = null;
            }

            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }

            speedProvider = null;
            equalizerProvider = null;
            pitchProvider = null;
            liveWaveformProvider = null;
            GlitchProvider = null;
            peaks = null;
        }

        public void Dispose()
        {
            CleanupAudio();
            renderTimer.Dispose();

            if (targetPictureBox != null)
            {
                targetPictureBox.Paint -= OnPaint;
                targetPictureBox.MouseDown -= OnMouseDown;
            }
        }
    }
}
