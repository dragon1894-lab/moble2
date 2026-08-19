using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using static DJing.DJform;

namespace DJing
{
    public class TurntableDeck : IDisposable
    {
        private PictureBox targetPictureBox;
        private Bitmap? lpImage;

        // 회전 각도 및 마우스 드래그 상태
        private float currentAngle = 0f;
        private bool isDragging = false;
        private float lastMouseAngle = 0f;

        // 🔗 외부 WaveformDeck 연결 참조
        private WaveformDeck? linkedWaveform;
        private System.Windows.Forms.Timer autoRotateTimer;

        public TurntableDeck(PictureBox pictureBox)
        {
            this.targetPictureBox = pictureBox;

            LoadTurntableImage();

            this.targetPictureBox.Paint += OnPaint;
            this.targetPictureBox.MouseDown += OnMouseDown;
            this.targetPictureBox.MouseMove += OnMouseMove;
            this.targetPictureBox.MouseUp += OnMouseUp;

            autoRotateTimer = new System.Windows.Forms.Timer();
            autoRotateTimer.Interval = 20; // 약 50 FPS
            autoRotateTimer.Tick += (object? sender, EventArgs e) =>
            {
                // 파형이 현재 재생 중일 때만 회전
                if (!isDragging && linkedWaveform != null && linkedWaveform.IsPlaying)
                {
                    currentAngle = (currentAngle + 3f) % 360f;
                    targetPictureBox.Invalidate();
                }
            };
            autoRotateTimer.Start();
        }

        // 파형 덱과 연동해주는 메서드
        public void LinkWaveform(WaveformDeck waveform)
        {
            this.linkedWaveform = waveform;
        }

        #region 🖼️ 이미지 로드
        private void LoadTurntableImage()
        {
            try
            {
                object? obj = Properties.Resources.ResourceManager.GetObject("TurnTable");
                if (obj is Bitmap resourceImage)
                {
                    lpImage = new Bitmap(resourceImage);
                    return;
                }
            }
            catch { }

            string debugFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TurnTable.png");
            if (File.Exists(debugFolderPath))
            {
                using (var ms = new MemoryStream(File.ReadAllBytes(debugFolderPath)))
                {
                    lpImage = new Bitmap(ms);
                }
            }
        }
        #endregion

        #region 🎨 렌더링
        private void OnPaint(object? sender, PaintEventArgs e)
        {
            if (lpImage == null || targetPictureBox == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int width = targetPictureBox.Width;
            int height = targetPictureBox.Height;
            float centerX = width / 2f;
            float centerY = height / 2f;
            float size = Math.Min(width, height) - 4f;

            g.TranslateTransform(centerX, centerY);
            g.RotateTransform(currentAngle);
            g.DrawImage(lpImage, -size / 2f, -size / 2f, size, size);
            g.ResetTransform();
        }
        #endregion

        #region 🖱️ 마우스 스크래치 조작 (WaveformDeck 오디오 제어)
        private float GetAngleFromCenter(Point point)
        {
            float centerX = targetPictureBox.Width / 2f;
            float centerY = targetPictureBox.Height / 2f;
            return (float)(Math.Atan2(point.Y - centerY, point.X - centerX) * (180.0 / Math.PI));
        }

        private void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && linkedWaveform != null)
            {
                isDragging = true;
                lastMouseAngle = GetAngleFromCenter(e.Location);

                // 손으로 잡았을 때 Waveform 소리 일시 정지
                linkedWaveform.Pause();
            }
        }

        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (isDragging && linkedWaveform != null && linkedWaveform.AudioFile != null)
            {
                float currentMouseAngle = GetAngleFromCenter(e.Location);
                float deltaAngle = currentMouseAngle - lastMouseAngle;

                if (deltaAngle > 180) deltaAngle -= 360;
                else if (deltaAngle < -180) deltaAngle += 360;

                currentAngle += deltaAngle;
                lastMouseAngle = currentMouseAngle;

                // LP판 돌리는 각도에 맞춰 Waveform의 재생 위치 이동
                var audio = linkedWaveform.AudioFile;
                double changeSeconds = deltaAngle * 0.01;
                double targetPosition = audio.CurrentTime.TotalSeconds + changeSeconds;

                if (targetPosition >= 0 && targetPosition < audio.TotalTime.TotalSeconds)
                {
                    audio.CurrentTime = TimeSpan.FromSeconds(targetPosition);
                }

                targetPictureBox.Invalidate();
            }
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isDragging)
            {
                isDragging = false;

                // 손을 떼면 Waveform 소리 다시 재생
                linkedWaveform?.Play();
            }
        }
        #endregion

        public void Dispose()
        {
            autoRotateTimer?.Dispose();
            lpImage?.Dispose();

            if (targetPictureBox != null)
            {
                targetPictureBox.Paint -= OnPaint;
                targetPictureBox.MouseDown -= OnMouseDown;
                targetPictureBox.MouseMove -= OnMouseMove;
                targetPictureBox.MouseUp -= OnMouseUp;
            }
        }
    }
}
