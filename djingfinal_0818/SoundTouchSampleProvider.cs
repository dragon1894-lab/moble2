using System;
using NAudio.Wave;
using SoundTouch;

namespace DJing
{
    /// <summary>
    /// 정상 동작이 확인된 WinFormsApp_0812의 SoundTouch 스트리밍 구현.
    /// 피치는 재생 속도를 바꾸지 않고 -20~+20 반음 범위에서 변경된다.
    /// </summary>
    public sealed class SoundTouchSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private readonly SoundTouchProcessor soundTouch;
        private float pitchSemitones;

        public SoundTouchSampleProvider(ISampleProvider source, float initialPitchSemitones = 0f)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            soundTouch = new SoundTouchProcessor
            {
                SampleRate = source.WaveFormat.SampleRate,
                Channels = source.WaveFormat.Channels
            };

            SetPitchSemitones(initialPitchSemitones);
        }

        public WaveFormat WaveFormat => source.WaveFormat;

        public void SetPitchSemitones(float semitones)
        {
            pitchSemitones = Math.Clamp(semitones, -20f, 20f);
            soundTouch.PitchSemiTones = pitchSemitones;
        }

        /// <summary>
        /// 탐색 또는 곡 처음으로 이동할 때 SoundTouch 내부에 남은 샘플을 제거한다.
        /// </summary>
        public void Reset()
        {
            soundTouch.Clear();
            soundTouch.PitchSemiTones = pitchSemitones;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0 || count < 0 || offset + count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count));

            // 정상 폼과 동일하게 원음은 SoundTouch를 우회한다.
            if (Math.Abs(pitchSemitones) < 0.01f)
            {
                return source.Read(buffer, offset, count);
            }

            int channels = WaveFormat.Channels;
            int targetFrames = count / channels;
            int totalFramesReceived = 0;
            float[] inputBuffer = new float[2048 * channels];

            while (totalFramesReceived < targetFrames)
            {
                int framesNeeded = targetFrames - totalFramesReceived;
                float[] outputBuffer = new float[framesNeeded * channels];
                int framesReceived = soundTouch.ReceiveSamples(outputBuffer, framesNeeded);

                if (framesReceived > 0)
                {
                    int destination = offset + totalFramesReceived * channels;
                    int samplesToCopy = framesReceived * channels;
                    for (int i = 0; i < samplesToCopy; i++)
                    {
                        buffer[destination + i] = outputBuffer[i];
                    }

                    totalFramesReceived += framesReceived;
                }

                if (totalFramesReceived >= targetFrames) continue;

                int samplesRead = source.Read(inputBuffer, 0, inputBuffer.Length);
                if (samplesRead == 0)
                {
                    soundTouch.Flush();
                    int remainingFrames = targetFrames - totalFramesReceived;
                    float[] finalBuffer = new float[remainingFrames * channels];
                    int finalFrames = soundTouch.ReceiveSamples(finalBuffer, remainingFrames);

                    if (finalFrames > 0)
                    {
                        int destination = offset + totalFramesReceived * channels;
                        int samplesToCopy = finalFrames * channels;
                        for (int i = 0; i < samplesToCopy; i++)
                        {
                            buffer[destination + i] = finalBuffer[i];
                        }

                        totalFramesReceived += finalFrames;
                    }

                    break;
                }

                soundTouch.PutSamples(inputBuffer, samplesRead / channels);
            }

            return totalFramesReceived * channels;
        }
    }
}
