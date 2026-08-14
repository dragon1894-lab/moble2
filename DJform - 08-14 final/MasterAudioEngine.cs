using System;
using System.Diagnostics;
using System.IO;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace DJform
{
    /// <summary>좌우 덱을 하나로 믹스하고 최종 출력 및 녹음을 담당한다.</summary>
    public sealed class MasterAudioEngine : IDisposable
    {
        public const int SampleRate = 44100;
        public const int Channels = 2;
        private readonly MixingSampleProvider mixer;
        private readonly RecordingTapSampleProvider recordingTap;
        private readonly DirectSoundOut output;

        public MasterAudioEngine()
        {
            var format = WaveFormat.CreateIeeeFloatWaveFormat(SampleRate, Channels);
            mixer = new MixingSampleProvider(format) { ReadFully = true };
            recordingTap = new RecordingTapSampleProvider(mixer);
            output = new DirectSoundOut();
            output.Init(recordingTap.ToWaveProvider());
            output.Play();
        }

        public bool IsRecording => recordingTap.IsRecording;
        public TimeSpan RecordingTime => recordingTap.RecordingTime;

        public void AddInput(ISampleProvider input) => mixer.AddMixerInput(input);
        public void RemoveInput(ISampleProvider input) => mixer.RemoveMixerInput(input);
        public void StartRecording() => recordingTap.Start();
        public string? StopRecording() => recordingTap.Stop();

        public static void EncodeMp3(string sourceWavePath, string destinationMp3Path)
        {
            using var reader = new WaveFileReader(sourceWavePath);
            MediaFoundationEncoder.EncodeToMp3(reader, destinationMp3Path, 192000);
        }

        public void Dispose()
        {
            string? unfinishedRecording = recordingTap.Stop();
            output.Stop();
            output.Dispose();
            recordingTap.Dispose();
            if (unfinishedRecording != null && File.Exists(unfinishedRecording))
                File.Delete(unfinishedRecording);
        }

        private sealed class RecordingTapSampleProvider : ISampleProvider, IDisposable
        {
            private readonly ISampleProvider source;
            private readonly object writerLock = new object();
            private readonly Stopwatch stopwatch = new Stopwatch();
            private WaveFileWriter? writer;
            private string? temporaryPath;

            public RecordingTapSampleProvider(ISampleProvider source)
            {
                this.source = source;
                WaveFormat = source.WaveFormat;
            }

            public WaveFormat WaveFormat { get; }
            public bool IsRecording { get { lock (writerLock) return writer != null; } }
            public TimeSpan RecordingTime => stopwatch.Elapsed;

            public int Read(float[] buffer, int offset, int count)
            {
                int read = source.Read(buffer, offset, count);
                lock (writerLock)
                {
                    writer?.WriteSamples(buffer, offset, read);
                }
                return read;
            }

            public void Start()
            {
                lock (writerLock)
                {
                    if (writer != null) return;
                    temporaryPath = Path.Combine(Path.GetTempPath(), $"DJform_{Guid.NewGuid():N}.wav");
                    writer = new WaveFileWriter(temporaryPath, WaveFormat);
                    stopwatch.Restart();
                }
            }

            public string? Stop()
            {
                lock (writerLock)
                {
                    if (writer == null) return null;
                    stopwatch.Stop();
                    writer.Dispose();
                    writer = null;
                    string? result = temporaryPath;
                    temporaryPath = null;
                    return result;
                }
            }

            public void Dispose()
            {
                string? path = Stop();
                if (path != null && File.Exists(path)) File.Delete(path);
            }
        }
    }
}
