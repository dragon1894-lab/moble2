using System;
using NAudio.Dsp;
using NAudio.Wave;

namespace WinFormsApp_0812
{
    public class EqualizerEngine : ISampleProvider, IDisposable
    {
        private AudioFileReader reader;
        private SoundTouchSampleProvider pitchShifter; // SoundTouch 기반 클래스로 변경
        private WaveOutEvent waveOut;

        private BiQuadFilter midFilterL, midFilterR;
        private float midGainDb = 0f;

        public WaveFormat WaveFormat => reader?.WaveFormat;

        public void LoadAudio(string filePath)
        {
            Stop();
            reader = new AudioFileReader(filePath);

            // SoundTouchSampleProvider 연결
            pitchShifter = new SoundTouchSampleProvider(reader);

            UpdateMidFilter();

            waveOut = new WaveOutEvent
            {
                DesiredLatency = 150,
                NumberOfBuffers = 3
            };
            waveOut.Init(this);
        }

        public void Play() => waveOut?.Play();

        public void Stop()
        {
            waveOut?.Stop();
            waveOut?.Dispose();
            reader?.Dispose();
            waveOut = null;
            reader = null;
            pitchShifter = null;
        }

        public void SetPitch(float semitones)
        {
            pitchShifter?.SetPitchSemitones(semitones);
        }

        public void SetMidGain(float gainDb)
        {
            midGainDb = gainDb;
            UpdateMidFilter();
        }

        private void UpdateMidFilter()
        {
            if (reader == null) return;
            midFilterL = BiQuadFilter.PeakingEQ(WaveFormat.SampleRate, 1000f, 0.5f, midGainDb);
            midFilterR = BiQuadFilter.PeakingEQ(WaveFormat.SampleRate, 1000f, 0.5f, midGainDb);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (pitchShifter == null) return 0;

            // 1. SoundTouch 엔진으로 피치 변환 샘플 가져오기
            int samplesRead = pitchShifter.Read(buffer, offset, count);

            // 2. Mid EQ 적용
            for (int i = 0; i < samplesRead; i += WaveFormat.Channels)
            {
                float sampleL = buffer[offset + i];
                float filteredL = sampleL;
                if (midFilterL != null) filteredL = midFilterL.Transform(filteredL);
                buffer[offset + i] = float.IsNaN(filteredL) ? sampleL : filteredL;

                if (WaveFormat.Channels > 1)
                {
                    float sampleR = buffer[offset + i + 1];
                    float filteredR = sampleR;
                    if (midFilterR != null) filteredR = midFilterR.Transform(filteredR);
                    buffer[offset + i + 1] = float.IsNaN(filteredR) ? sampleR : filteredR;
                }
            }

            return samplesRead;
        }

        public void Dispose() => Stop();
    }
}