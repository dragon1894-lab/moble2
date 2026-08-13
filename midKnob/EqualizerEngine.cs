using System;
using NAudio.Dsp;
using NAudio.Wave;

namespace WinFormsApp_0812
{
    public class EqualizerEngine : ISampleProvider, IDisposable
    {
        private AudioFileReader reader;
        private WaveOutEvent waveOut;

        // Mid 필터 인스턴스 (좌/우 채널용)
        private BiQuadFilter midFilterL, midFilterR;

        // 현재 Mid EQ 설정값 (-30dB ~ +12dB 권장)
        private float midGainDb = 0f;

        public WaveFormat WaveFormat => reader?.WaveFormat;

        public void LoadAudio(string filePath)
        {
            Stop();
            reader = new AudioFileReader(filePath);

            UpdateMidFilter();

            waveOut = new WaveOutEvent();
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
        }

        #region 필터 설정 메서드

        // Mid 노브 조절 (Form1의 노브 이벤트에서 호출)
        public void SetMidGain(float gainDb)
        {
            midGainDb = gainDb;
            UpdateMidFilter();
        }

        #endregion

        #region NAudio BiQuadFilter 업데이트

        private void UpdateMidFilter()
        {
            if (reader == null) return;

            // Q = 0.5f로 설정하여 보컬 및 메인 중음역대 전체를 커버
            midFilterL = BiQuadFilter.PeakingEQ(WaveFormat.SampleRate, 1000f, 0.5f, midGainDb);
            midFilterR = BiQuadFilter.PeakingEQ(WaveFormat.SampleRate, 1000f, 0.5f, midGainDb);
        }

        #endregion

        // 실시간 샘플 변환 처리 (마스터 출력)
        public int Read(float[] buffer, int offset, int count)
        {
            if (reader == null) return 0;

            int samplesRead = reader.Read(buffer, offset, count);

            for (int i = 0; i < samplesRead; i += WaveFormat.Channels)
            {
                // L 채널 (Mid 필터만 적용)
                float sampleL = buffer[offset + i];
                float filteredL = sampleL;
                if (midFilterL != null) filteredL = midFilterL.Transform(filteredL);

                // NaN(숫자 오류) 검사 후 적용
                buffer[offset + i] = float.IsNaN(filteredL) ? sampleL : filteredL;

                // R 채널 (Mid 필터만 적용)
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