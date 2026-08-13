using System;
using NAudio.Wave;
using SoundTouch;

namespace WinFormsApp_0812
{
    public class SoundTouchSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private readonly SoundTouchProcessor soundTouch;
        private float pitchSemitones = 0f;

        public WaveFormat WaveFormat => source.WaveFormat;

        public SoundTouchSampleProvider(ISampleProvider source)
        {
            this.source = source;
            this.soundTouch = new SoundTouchProcessor();

            this.soundTouch.SampleRate = WaveFormat.SampleRate;
            this.soundTouch.Channels = WaveFormat.Channels;
        }

        public void SetPitchSemitones(float semitones)
        {
            this.pitchSemitones = semitones;
            this.soundTouch.PitchSemiTones = semitones;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            // 원음(0반음)일 경우 연산 없이 바로 출력
            if (Math.Abs(pitchSemitones) < 0.01f)
            {
                return source.Read(buffer, offset, count);
            }

            int channels = WaveFormat.Channels;
            int targetFrames = count / channels;
            int totalFramesReceived = 0;

            float[] tempReadBuffer = new float[2048 * channels];

            while (totalFramesReceived < targetFrames)
            {
                int framesNeeded = targetFrames - totalFramesReceived;
                float[] outBuffer = new float[framesNeeded * channels];
                int framesReceived = soundTouch.ReceiveSamples(outBuffer, framesNeeded);

                if (framesReceived > 0)
                {
                    int destIndex = offset + (totalFramesReceived * channels);
                    int samplesToCopy = framesReceived * channels;

                    // Array.Copy 대신 직접 대입으로 타입 미스매치 예외 방지
                    for (int i = 0; i < samplesToCopy; i++)
                    {
                        buffer[destIndex + i] = outBuffer[i];
                    }

                    totalFramesReceived += framesReceived;
                }

                if (totalFramesReceived < targetFrames)
                {
                    int read = source.Read(tempReadBuffer, 0, tempReadBuffer.Length);
                    if (read == 0)
                    {
                        soundTouch.Flush();
                        float[] finalBuffer = new float[(targetFrames - totalFramesReceived) * channels];
                        int finalFrames = soundTouch.ReceiveSamples(finalBuffer, targetFrames - totalFramesReceived);
                        if (finalFrames > 0)
                        {
                            int destIndex = offset + (totalFramesReceived * channels);
                            int samplesToCopy = finalFrames * channels;
                            for (int i = 0; i < samplesToCopy; i++)
                            {
                                buffer[destIndex + i] = finalBuffer[i];
                            }
                            totalFramesReceived += finalFrames;
                        }
                        break;
                    }

                    int framesRead = read / channels;
                    soundTouch.PutSamples(tempReadBuffer, framesRead);
                }
            }

            return totalFramesReceived * channels;
        }
    }
}