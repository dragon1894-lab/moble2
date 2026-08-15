using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DJing
{
    public class KnobControl
    {
        private const float MouseWheelStep = 0.02f;
        private PictureBox targetPictureBox;
        private Bitmap? knobImage;

        // 0.0 (최소) ~ 1.0 (최대) 값 관리 (기본 50% = 0.5)
        private float valueRatio = 0.5f;

        private bool isDragging = false;
        private Point lastMousePosition;

        public event Action<float>? ValueChanged;

        // 현재 노브 값 (0.0 ~ 1.0)
        public float Value
        {
            get => valueRatio;
            set
            {
                // 1. 기본 범위 제한 (0.0 ~ 1.0)
                float clamped = Math.Max(0f, Math.Min(1f, value));

                // 2. ★ ±3% (0.03) 범위 스냅(Snap) 로직 ★
                float center = 0.5f;
                float deadZone = 0.03f; // ±3% 오차 범위

                if (Math.Abs(clamped - center) <= deadZone)
                {
                    clamped = center;
                }

                // 값에 변화가 있을 때만 갱신
                if (valueRatio != clamped)
                {
                    valueRatio = clamped;
                    targetPictureBox?.Invalidate(); // 재그리기
                    ValueChanged?.Invoke(valueRatio);
                }
            }
        }

        public KnobControl(PictureBox pictureBox, Bitmap? customImage = null)
        {
            this.targetPictureBox = pictureBox;

            if (customImage != null)
            {
                this.knobImage = customImage;
            }
            else
            {
                this.knobImage = DrawKnobDesign(pictureBox.Width, pictureBox.Height);
            }

            // 이벤트 연결
            this.targetPictureBox.Paint += OnPaint;
            this.targetPictureBox.MouseDown += OnMouseDown;
            this.targetPictureBox.MouseMove += OnMouseMove;
            this.targetPictureBox.MouseUp += OnMouseUp;
            this.targetPictureBox.MouseDoubleClick += OnMouseDoubleClick; // ★ 더블클릭 이벤트 추가

            // PictureBox는 포커스를 직접 받지 못하는 경우가 있으므로 폼의
            // MouseWheel 이벤트를 받고 커서가 이 노브 위에 있을 때만 처리한다.
            Form? ownerForm = this.targetPictureBox.FindForm();
            if (ownerForm != null)
            {
                ownerForm.MouseWheel += OnMouseWheel;
            }
        }

        #region 🎨 1. 기본 노브 디자인 (이미지 없을 시)
        private Bitmap DrawKnobDesign(int width, int height)
        {
            // 30x30 크기에 맞게 여백을 최소화(2px)하여 26x26 비트맵 생성
            int size = Math.Min(width, height) - 4;
            if (size <= 0) size = 10;

            Bitmap bmp = new Bitmap(size, size);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // [몸체]
                using (Brush b = new SolidBrush(Color.FromArgb(45, 45, 48)))
                    g.FillEllipse(b, 1, 1, size - 2, size - 2);

                // [테두리]
                using (Pen p = new Pen(Color.FromArgb(90, 90, 90), 1.5f))
                    g.DrawEllipse(p, 1, 1, size - 2, size - 2);

                // [중앙 지시선] 12시 방향 표시선 (DeepSkyBlue)
                using (Pen p = new Pen(Color.DeepSkyBlue, 2f))
                {
                    p.StartCap = LineCap.Round;
                    p.EndCap = LineCap.Round;
                    // 소형 크기에 맞춰 지시선 길이를 4px로 조율
                    g.DrawLine(p, size / 2f, size / 2f, size / 2f, 4);
                }
            }
            return bmp;
        }
        #endregion

        #region 🔄 2. Paint 이벤트 (10% 단위 눈금선 & 노브 회전)
        private void OnPaint(object? sender, PaintEventArgs e)
        {
            if (targetPictureBox == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int width = targetPictureBox.Width;
            int height = targetPictureBox.Height;
            float centerX = width / 2f;
            float centerY = height / 2f;

            // 눈금선(Tick Marks)을 그리는 로직을 완전히 제거했습니다.

            // ----------------------------------------------------
            // 노브 본체 회전하여 그리기
            // ----------------------------------------------------
            if (knobImage != null)
            {
                // 270도 회전 범위 (-135도 ~ +135도, 50%일 때 정확히 0도/12시)
                float currentAngle = -135f + (valueRatio * 270f);

                g.TranslateTransform(centerX, centerY);
                g.RotateTransform(currentAngle);
                g.DrawImage(knobImage, -knobImage.Width / 2f, -knobImage.Height / 2f);
                g.ResetTransform();
            }
        }
        #endregion

        #region 🖱️ 3. 마우스 드래그 & 더블클릭 조작
        private void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastMousePosition = e.Location;
            }
        }

        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                int deltaY = lastMousePosition.Y - e.Y;

                float sensitivity = 150f; // 마우스 이동 민감도 (픽셀)
                Value += deltaY / sensitivity; // Value 프로퍼티를 통해 ±3% Snap 자동 적용

                lastMousePosition = e.Location;
            }
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        // ★ 더블클릭 시 50% (0.5f)로 초기화하는 메서드
        private void OnMouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Value = 0.5f;
            }
        }

        private void OnMouseWheel(object? sender, MouseEventArgs e)
        {
            Rectangle knobBounds = targetPictureBox.RectangleToScreen(targetPictureBox.ClientRectangle);
            if (!knobBounds.Contains(Cursor.Position) || e.Delta == 0) return;

            int wheelNotches = e.Delta / SystemInformation.MouseWheelScrollDelta;
            if (wheelNotches == 0) wheelNotches = Math.Sign(e.Delta);
            Value += wheelNotches * MouseWheelStep;
        }
        #endregion
    }
}
