using System;
using NAudio.Wave;

namespace DJing
{
    /// <summary>마스터 믹서에서 덱별 재생/일시정지를 제어한다.</summary>
    public sealed class PlaybackGateSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private volatile bool isPlaying;

        public PlaybackGateSampleProvider(ISampleProvider source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            WaveFormat = source.WaveFormat;
        }

        public WaveFormat WaveFormat { get; }
        public bool IsPlaying { get => isPlaying; set => isPlaying = value; }

        public int Read(float[] buffer, int offset, int count)
        {
            if (!isPlaying)
            {
                Array.Clear(buffer, offset, count);
                return count;
            }

            int read = source.Read(buffer, offset, count);
            if (read < count)
            {
                Array.Clear(buffer, offset + read, count - read);
                if (read == 0) isPlaying = false;
            }
            return count;
        }
    }
}
