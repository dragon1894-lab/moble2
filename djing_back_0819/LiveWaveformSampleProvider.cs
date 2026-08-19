using System;
using NAudio.Wave;

namespace DJing
{
    /// <summary>
    /// 최종 출력 샘플을 통과시키면서 최근 모노 파형을 보관한다.
    /// 오디오 출력에는 어떤 변경도 가하지 않는다.
    /// </summary>
    public sealed class LiveWaveformSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private readonly float[] ringBuffer;
        private readonly object bufferLock = new object();
        private int writePosition;
        private int storedSamples;

        public LiveWaveformSampleProvider(ISampleProvider source, double historySeconds = 0.12)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            WaveFormat = source.WaveFormat;
            ringBuffer = new float[Math.Max(512, (int)(WaveFormat.SampleRate * historySeconds))];
        }

        public WaveFormat WaveFormat { get; }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = source.Read(buffer, offset, count);
            int channels = WaveFormat.Channels;

            lock (bufferLock)
            {
                for (int frame = 0; frame < samplesRead; frame += channels)
                {
                    float mono = 0f;
                    int frameChannels = Math.Min(channels, samplesRead - frame);
                    for (int channel = 0; channel < frameChannels; channel++)
                        mono += buffer[offset + frame + channel];
                    mono /= Math.Max(1, frameChannels);

                    ringBuffer[writePosition] = mono;
                    writePosition = (writePosition + 1) % ringBuffer.Length;
                    if (storedSamples < ringBuffer.Length) storedSamples++;
                }
            }

            return samplesRead;
        }

        public int CopyRecentSamples(float[] destination)
        {
            lock (bufferLock)
            {
                int count = Math.Min(destination.Length, storedSamples);
                int start = (writePosition - count + ringBuffer.Length) % ringBuffer.Length;
                for (int i = 0; i < count; i++)
                    destination[destination.Length - count + i] = ringBuffer[(start + i) % ringBuffer.Length];
                return count;
            }
        }
    }
}

