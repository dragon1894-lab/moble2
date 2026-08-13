using System;
using NAudio.Wave;
using SoundTouch; // SoundTouch.NET 패키지

namespace DJform
{
    public static class AudioBpmHelper
    {
        /// <summary>
        /// SoundTouch 엔진을 사용해 오디오 파일의 BPM을 감지합니다.
        /// </summary>
        public static float DetectBpm(string filePath)
        {
            try
            {
                using (var reader = new AudioFileReader(filePath))
                {
                    int sampleRate = reader.WaveFormat.SampleRate;
                    int channels = reader.WaveFormat.Channels;

                    // SoundTouch.NET 내부의 정식 BpmDetect 객체 생성
                    var detector = new BpmDetect(channels, sampleRate);

                    // 약 35초 분량 샘플 추출 (빠른 연산을 위함)
                    int samplesToRead = sampleRate * channels * 35;
                    float[] buffer = new float[samplesToRead];
                    int readCount = reader.Read(buffer, 0, samplesToRead);

                    if (readCount > 0)
                    {
                        // BpmDetect 객체에 샘플 전달 (채널 수로 나눈 샘플 수 입력)
                        detector.InputSamples(buffer, readCount / channels);
                    }

                    // GetBpm() 메서드로 감지된 수치 추출
                    float detectedBpm = detector.GetBpm();

                    if (detectedBpm <= 0) return 120.0f;

                    return (float)Math.Round(detectedBpm, 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BPM 분석 에러: {ex.Message}");
                return 120.0f; // 실패 시 기본 120.0 반환
            }
        }
    }
}