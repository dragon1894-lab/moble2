using System;
using NAudio.Wave;

namespace DJform
{
    /// <summary>
    /// 실시간 음원 속도(BPM) 조절 및 안정적인 리샘플링 처리 클래스
    /// </summary>
    public class SpeedSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private float playbackRate = 1.0f;
        private float[] sourceBuffer = new float[0];

        public SpeedSampleProvider(ISampleProvider source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.WaveFormat = source.WaveFormat;
        }

        public WaveFormat WaveFormat { get; }

        public float PlaybackRate
        {
            get => playbackRate;
            set => playbackRate = Math.Clamp(value, 0.5f, 2.0f); // 0.5배속 ~ 2.0배속
        }

        public int Read(float[] buffer, int offset, int count)
        {
            // 1.0배속일 때는 원본 스트림을 그대로 바이패스 (연산 오차 및 속도 저하 방지)
            if (Math.Abs(playbackRate - 1.0f) < 0.001f)
            {
                return source.Read(buffer, offset, count);
            }

            int channels = WaveFormat.Channels;
            int framesRequested = count / channels;

            // 필요한 소스 프레임 수 계산
            int sourceFramesNeeded = (int)Math.Ceiling(framesRequested * playbackRate) + 2;
            int sourceSamplesNeeded = sourceFramesNeeded * channels;

            if (sourceBuffer.Length < sourceSamplesNeeded)
            {
                sourceBuffer = new float[sourceSamplesNeeded];
            }

            int sourceSamplesRead = source.Read(sourceBuffer, 0, sourceSamplesNeeded);
            int sourceFramesRead = sourceSamplesRead / channels;

            if (sourceFramesRead == 0)
            {
                // 데이터가 더 이상 없으면 0 채우기 후 종료
                Array.Clear(buffer, offset, count);
                return 0;
            }

            int framesWritten = 0;
            for (int i = 0; i < framesRequested; i++)
            {
                float srcFramePos = i * playbackRate;
                int frame1 = (int)srcFramePos;
                int frame2 = frame1 + 1;
                float frac = srcFramePos - frame1;

                // 읽어온 영역을 벗어나는 경우 마지막 프레임 값으로 안전하게 패딩
                if (frame1 >= sourceFramesRead)
                {
                    frame1 = Math.Max(0, sourceFramesRead - 1);
                    frame2 = frame1;
                    frac = 0f;
                }
                else if (frame2 >= sourceFramesRead)
                {
                    frame2 = frame1;
                }

                int outIndex = offset + (i * channels);
                int inIndex1 = frame1 * channels;
                int inIndex2 = frame2 * channels;

                for (int ch = 0; ch < channels; ch++)
                {
                    float s1 = sourceBuffer[inIndex1 + ch];
                    float s2 = sourceBuffer[inIndex2 + ch];
                    buffer[outIndex + ch] = s1 + (s2 - s1) * frac;
                }

                framesWritten++;
            }

            return framesWritten * channels;
        }
    }
}