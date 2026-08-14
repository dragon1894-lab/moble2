using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;

namespace DJform
{
    /// <summary>
    /// 개별 덱의 오디오 재생, BPM 변환, 파형 렌더링을 담당하는 클래스
    /// </summary>
    public class WaveformDeck : IDisposable
    {
        private readonly PictureBox targetPictureBox;
        private AudioFileReader? audioFile;
        private SpeedSampleProvider? speedProvider;
        private DeckEqualizerSampleProvider? equalizerProvider;
        private SoundTouchSampleProvider? pitchProvider;
        private IWavePlayer? waveOut;
        private float[]? peaks;
        private readonly System.Windows.Forms.Timer renderTimer;
        public TimeSpan CurrentTime => audioFile?.CurrentTime ?? TimeSpan.Zero;
        public TimeSpan TotalTime => audioFile?.TotalTime ?? TimeSpan.Zero;
        public event Action? PositionChanged;
        public GlitchStutterProvider? GlitchProvider { get; private set; }

        public AudioFileReader? AudioFile => audioFile;
        public bool IsPlaying => waveOut != null && waveOut.PlaybackState == PlaybackState.Playing;

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

        public WaveformDeck(PictureBox pictureBox)
        {
            targetPictureBox = pictureBox;

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

            // WinFormsApp2와 동일한 출력 드라이버.
            waveOut = new DirectSoundOut();

            waveOut.PlaybackStopped += (s, e) =>
            {
                renderTimer.Stop();
                if (e.Exception != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[Playback Error] {e.Exception.Message}");
                }
            };

            waveOut.Init(GlitchProvider.ToWaveProvider());

            targetPictureBox.Invalidate();
        }

        public void Play()
        {
            if (waveOut == null || audioFile == null) return;

            try
            {
                if (audioFile.Position >= audioFile.Length)
                {
                    audioFile.Position = 0;
                    pitchProvider?.Reset();
                }

                if (waveOut.PlaybackState != PlaybackState.Playing)
                {
                    waveOut.Play();
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
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Pause();
                renderTimer.Stop();
                targetPictureBox.Invalidate();
            }
        }

        public void Stop()
        {
            if (waveOut != null && audioFile != null)
            {
                waveOut.Stop();
                audioFile.CurrentTime = TimeSpan.Zero;
                renderTimer.Stop();
                targetPictureBox.Invalidate();
            }
        }

        private void OnPaint(object? sender, PaintEventArgs e)
        {
            if (peaks == null || audioFile == null || peaks.Length == 0) return;

            e.Graphics.Clear(Color.FromArgb(20, 22, 32));

            int currentWidth = targetPictureBox.Width;
            int currentHeight = targetPictureBox.Height;

            double progress = 0;
            if (audioFile.TotalTime.TotalMilliseconds > 0)
            {
                progress = audioFile.CurrentTime.TotalMilliseconds / audioFile.TotalTime.TotalMilliseconds;
            }
            int progressX = (int)(currentWidth * progress);

            float stepX = (float)currentWidth / peaks.Length;
            float penWidth = Math.Max(1.0f, stepX);

            for (int i = 0; i < peaks.Length; i++)
            {
                float x = i * stepX;
                int barHeight = (int)(peaks[i] * (currentHeight * 0.6f));

                Color barColor = (x < progressX) ? Color.FromArgb(255, 120, 0) : Color.FromArgb(80, 85, 100);

                using (Pen pen = new Pen(barColor, penWidth))
                {
                    e.Graphics.DrawLine(pen, x, currentHeight, x, currentHeight - barHeight);
                }
            }

            using (Pen whitePen = new Pen(Color.White, 2))
            {
                e.Graphics.DrawLine(whitePen, progressX, 0, progressX, currentHeight);
            }
        }

        private void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (audioFile == null || audioFile.TotalTime.TotalMilliseconds <= 0) return;

            double targetRatio = (double)e.X / targetPictureBox.Width;
            targetRatio = Math.Max(0, Math.Min(1, targetRatio));

            audioFile.CurrentTime = TimeSpan.FromMilliseconds(audioFile.TotalTime.TotalMilliseconds * targetRatio);
            pitchProvider?.Reset();
            if (autoLoopActive)
            {
                autoLoopStart = audioFile.CurrentTime;
                RecalculateAutoLoopEnd();
            }
            targetPictureBox.Invalidate();
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

        public void CleanupAudio()
        {
            renderTimer.Stop();
            autoLoopActive = false;

            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }

            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }

            speedProvider = null;
            equalizerProvider = null;
            pitchProvider = null;
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
