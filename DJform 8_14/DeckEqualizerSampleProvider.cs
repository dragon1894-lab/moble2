using System;
using NAudio.Dsp;
using NAudio.Wave;

namespace DJform
{
    /// <summary>
    /// 덱 하나의 Bass, Mid, DJ Filter를 통합 처리한다.
    /// Bass/Filter 값은 -1~+1이며 0에서 원음을 유지한다.
    /// </summary>
    public sealed class DeckEqualizerSampleProvider : ISampleProvider
    {
        private const float MidCenterFrequency = 1000f;
        private const float MidQ = 0.5f;

        private readonly ISampleProvider source;
        private readonly object filterLock = new object();
        private BiQuadFilter[] midFilters;
        private BiQuadFilter[]? bassFilters;
        private BiQuadFilter[]? filterPass1;
        private BiQuadFilter[]? filterPass2;
        private float midGainDb;
        private float bassValue;
        private float filterValue;

        public DeckEqualizerSampleProvider(
            ISampleProvider source,
            float initialMidGainDb = 0f,
            float initialBassValue = 0f,
            float initialFilterValue = 0f)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            WaveFormat = source.WaveFormat;
            midGainDb = Math.Clamp(initialMidGainDb, -30f, 12f);
            bassValue = Math.Clamp(initialBassValue, -1f, 1f);
            filterValue = Math.Clamp(initialFilterValue, -1f, 1f);
            midFilters = CreateMidFilters(midGainDb);
            CreateBassFilters();
            CreateDjFilters();
        }

        public WaveFormat WaveFormat { get; }

        public void SetMidGain(float gainDb)
        {
            gainDb = Math.Clamp(gainDb, -30f, 12f);
            lock (filterLock)
            {
                if (Math.Abs(midGainDb - gainDb) < 0.01f) return;
                midGainDb = gainDb;
                midFilters = CreateMidFilters(midGainDb);
            }
        }

        public void SetBass(float value)
        {
            value = Math.Clamp(value, -1f, 1f);
            lock (filterLock)
            {
                if (Math.Abs(bassValue - value) < 0.001f) return;
                bassValue = value;
                CreateBassFilters();
            }
        }

        public void SetFilter(float value)
        {
            value = Math.Clamp(value, -1f, 1f);
            lock (filterLock)
            {
                if (Math.Abs(filterValue - value) < 0.001f) return;
                filterValue = value;
                CreateDjFilters();
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = source.Read(buffer, offset, count);
            if (samplesRead == 0) return 0;

            lock (filterLock)
            {
                int channels = WaveFormat.Channels;
                for (int i = 0; i < samplesRead; i++)
                {
                    int channel = i % channels;
                    int sampleIndex = offset + i;
                    float sample = buffer[sampleIndex];

                    if (filterPass1 != null && filterPass2 != null)
                    {
                        sample = filterPass1[channel].Transform(sample);
                        sample = filterPass2[channel].Transform(sample);
                    }

                    if (bassFilters != null)
                    {
                        sample = bassFilters[channel].Transform(sample);
                    }

                    sample = midFilters[channel].Transform(sample);
                    if (!float.IsNaN(sample) && !float.IsInfinity(sample))
                    {
                        buffer[sampleIndex] = Math.Clamp(sample, -1f, 1f);
                    }
                }
            }

            return samplesRead;
        }

        private BiQuadFilter[] CreateMidFilters(float gainDb)
        {
            var result = new BiQuadFilter[WaveFormat.Channels];
            for (int channel = 0; channel < result.Length; channel++)
            {
                result[channel] = BiQuadFilter.PeakingEQ(
                    WaveFormat.SampleRate,
                    MidCenterFrequency,
                    MidQ,
                    gainDb);
            }
            return result;
        }

        private void CreateBassFilters()
        {
            if (Math.Abs(bassValue) < 0.05f)
            {
                bassFilters = null;
                return;
            }

            bassFilters = new BiQuadFilter[WaveFormat.Channels];
            float gainDb = bassValue < 0f ? bassValue * 40f : bassValue * 18f;
            for (int channel = 0; channel < bassFilters.Length; channel++)
            {
                bassFilters[channel] = BiQuadFilter.LowShelf(
                    WaveFormat.SampleRate,
                    300f,
                    1.5f,
                    gainDb);
            }
        }

        private void CreateDjFilters()
        {
            if (Math.Abs(filterValue) < 0.05f)
            {
                filterPass1 = null;
                filterPass2 = null;
                return;
            }

            int channels = WaveFormat.Channels;
            int sampleRate = WaveFormat.SampleRate;
            filterPass1 = new BiQuadFilter[channels];
            filterPass2 = new BiQuadFilter[channels];

            if (filterValue < 0f)
            {
                float amount = -filterValue;
                float cutoff = 20000f * (float)Math.Pow(70f / 20000f, amount);
                cutoff = Math.Clamp(cutoff, 30f, sampleRate / 2f - 100f);
                for (int channel = 0; channel < channels; channel++)
                {
                    filterPass1[channel] = BiQuadFilter.LowPassFilter(sampleRate, cutoff, 2.2f);
                    filterPass2[channel] = BiQuadFilter.LowPassFilter(sampleRate, cutoff, 2.2f);
                }
            }
            else
            {
                float cutoff = 20f * (float)Math.Pow(6500f / 20f, filterValue);
                cutoff = Math.Clamp(cutoff, 20f, sampleRate / 2f - 100f);
                for (int channel = 0; channel < channels; channel++)
                {
                    filterPass1[channel] = BiQuadFilter.HighPassFilter(sampleRate, cutoff, 3.5f);
                    filterPass2[channel] = BiQuadFilter.HighPassFilter(sampleRate, cutoff, 3.5f);
                }
            }
        }
    }
}
