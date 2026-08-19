using NAudio.Wave;
using System;

namespace DJing
{
    public class GlitchStutterProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private readonly float[] ringBuffer;
        private int ringWritePos;

        // 스터터
        private bool isStuttering;
        private int stutterStartPos;
        private int stutterLength;
        private int stutterReadOffset;
        private int stutterRemainingSamples;
        private bool isReverse;

        // 비트크러시
        private float crushLevel = 0f;
        private float lastCrushedSample = 0f;
        private int downsampleCounter = 0;

        // 에코(Delay)
        private float echoLevel = 0f;
        private readonly float[] delayBuffer;
        private int delayWritePos;
        private readonly int delaySampleLength;

        // 플랜저
        private float flangerLevel = 0f;
        private readonly float[] flangerBuffer;
        private int flangerWritePos;
        private float flangerPhase = 0f;

        public WaveFormat WaveFormat => source.WaveFormat;

        public GlitchStutterProvider(ISampleProvider source, float maxBufferSeconds = 1.0f)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            int channels = source.WaveFormat.Channels;
            int sampleRate = source.WaveFormat.SampleRate;

            // 스터터 버퍼 생성
            int bufferSize = (int)(sampleRate * channels * maxBufferSeconds);
            this.ringBuffer = new float[Math.Max(1024, bufferSize)];

            // 에코 버퍼 생성 (250ms 지연)
            this.delaySampleLength = (int)(sampleRate * channels * 0.25f);
            this.delaySampleLength -= this.delaySampleLength % channels;
            this.delayBuffer = new float[sampleRate * channels * 2];

            // 플랜저 버퍼
            int flangerBufSize = sampleRate * channels;
            this.flangerBuffer = new float[flangerBufSize];
        }

        public void SetCrush(float level) => this.crushLevel = Math.Clamp(level, 0f, 100f);
        public void SetEcho(float level) => this.echoLevel = Math.Clamp(level, 0f, 100f);
        public void SetFlanger(float level) => this.flangerLevel = Math.Clamp(level, 0f, 100f);

        public void ResetAllEffects()
        {
            StopStutter();
            SetCrush(0);
            SetEcho(0);
            SetFlanger(0);
        }

        public void TriggerStutter(float sliceMs, float totalMs, bool reverse = false)
        {
            int channels = WaveFormat.Channels;
            int samplesPerMs = (WaveFormat.SampleRate * channels) / 1000;

            int calculatedLength = Math.Max(channels, (int)(sliceMs * samplesPerMs));
            calculatedLength -= calculatedLength % channels;

            if (isStuttering)
            {
                this.stutterLength = calculatedLength;
                return;
            }

            this.stutterLength = calculatedLength;
            this.stutterRemainingSamples = (int)(totalMs * samplesPerMs);

            int rawStart = ringWritePos - stutterLength;
            this.stutterStartPos = ((rawStart % ringBuffer.Length) + ringBuffer.Length) % ringBuffer.Length;
            this.stutterStartPos -= this.stutterStartPos % channels;

            this.stutterReadOffset = reverse ? stutterLength - channels : 0;
            this.isReverse = reverse;
            this.isStuttering = true;
        }

        public void StopStutter()
        {
            this.isStuttering = false;
            this.stutterRemainingSamples = 0;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            // 하위 노드(SoundTouch/SpeedProvider)로부터 오디오 데이터 수신
            int samplesRead = source.Read(buffer, offset, count);
            if (samplesRead == 0) return 0;

            int channels = WaveFormat.Channels;

            for (int i = 0; i < samplesRead; i++)
            {
                int currentIdx = offset + i;
                float inputSample = buffer[currentIdx];
                float outSample = inputSample;

                // 1. 스터터 연산
                if (isStuttering && stutterLength > 0)
                {
                    int rawReadIdx = stutterStartPos + stutterReadOffset;
                    int readIndex = ((rawReadIdx % ringBuffer.Length) + ringBuffer.Length) % ringBuffer.Length;

                    outSample = ringBuffer[readIndex];

                    if (isReverse)
                    {
                        stutterReadOffset -= 1;
                        if (stutterReadOffset < 0) stutterReadOffset = stutterLength - 1;
                    }
                    else
                    {
                        stutterReadOffset = (stutterReadOffset + 1) % stutterLength;
                    }

                    stutterRemainingSamples--;
                    if (stutterRemainingSamples <= 0) isStuttering = false;
                }
                else
                {
                    // 원음 RingBuffer 기록
                    ringBuffer[ringWritePos] = inputSample;
                    ringWritePos = (ringWritePos + 1) % ringBuffer.Length;
                }

                // 2. 비트크러시 연산
                if (crushLevel > 0)
                {
                    float ratio = crushLevel / 100f;
                    int holdFactor = (int)(1 + (ratio * 12));

                    downsampleCounter++;
                    if (downsampleCounter >= holdFactor)
                    {
                        downsampleCounter = 0;
                        float bits = 16f - (ratio * 13f);
                        float steps = MathF.Pow(2, Math.Max(2f, bits));
                        float quantized = MathF.Round(outSample * steps) / steps;
                        float drive = 1.0f + (ratio * 0.4f);
                        lastCrushedSample = Math.Clamp(quantized * drive, -1.0f, 1.0f);
                    }
                    outSample = lastCrushedSample;
                }
                else
                {
                    downsampleCounter = 0;
                    lastCrushedSample = outSample; // Crush 오프 시 현재 샘플로 동기화
                }

                // 3. 에코 연산
                if (echoLevel > 0)
                {
                    int delayReadIndex = (delayWritePos - delaySampleLength + delayBuffer.Length) % delayBuffer.Length;
                    float delayedSample = delayBuffer[delayReadIndex];

                    float feedback = (echoLevel / 100f) * 0.6f;
                    float wet = (echoLevel / 100f) * 0.5f;

                    delayBuffer[delayWritePos] = outSample + (delayedSample * feedback);
                    delayWritePos = (delayWritePos + 1) % delayBuffer.Length;

                    outSample = outSample + (delayedSample * wet);
                }

                // 4. 플랜저 연산
                flangerBuffer[flangerWritePos] = outSample;

                if (flangerLevel > 0)
                {
                    float ratio = flangerLevel / 100f;

                    flangerPhase += (float)(2.0 * Math.PI * 0.6 / (WaveFormat.SampleRate * channels));
                    if (flangerPhase > Math.PI * 2) flangerPhase -= (float)(Math.PI * 2);

                    float lfo = (MathF.Sin(flangerPhase) + 1.0f) * 0.5f;
                    float delayMs = 0.2f + (lfo * 5.8f);

                    int delaySamples = (int)(delayMs * (WaveFormat.SampleRate / 1000f)) * channels;

                    int readPos = ((flangerWritePos - delaySamples) % flangerBuffer.Length + flangerBuffer.Length) % flangerBuffer.Length;
                    float delayedSample = flangerBuffer[readPos];

                    float feedback = 0.75f * ratio;
                    flangerBuffer[flangerWritePos] += delayedSample * feedback;

                    float dryGain = 1.0f - (0.5f * ratio);
                    float wetGain = 0.5f * ratio;

                    outSample = (outSample * dryGain) + (delayedSample * wetGain);
                    outSample = Math.Clamp(outSample, -1.0f, 1.0f);
                }

                flangerWritePos = (flangerWritePos + 1) % flangerBuffer.Length;

                // 최종 샘플 버퍼 출력
                buffer[currentIdx] = outSample;
            }

            return samplesRead;
        }
    }
}