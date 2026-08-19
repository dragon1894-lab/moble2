#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DJing
{
    internal class WaveformControl : UserControl
    {
        public event EventHandler<double>? SeekRequested;

        private double progressRatio = 0.0;
        private bool isMouseDown = false;

        private float[]? waveData = null;

        public WaveformControl()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(30, 30, 30);

            this.MouseDown += WaveformControl_MouseDown;
            this.MouseMove += WaveformControl_MouseMove;
            this.MouseUp += (s, e) => isMouseDown = false;
        }

        public void SetWaveformData(float[] data)
        {
            this.waveData = data;
            this.Invalidate();
        }

        private void WaveformControl_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                HandleSeek(e.X);
            }
        }

        private void WaveformControl_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isMouseDown && e.Button == MouseButtons.Left)
            {
                HandleSeek(e.X);
            }
        }

        private void HandleSeek(int mouseX)
        {
            if (this.Width <= 0) return;

            double ratio = (double)mouseX / (double)this.Width;
            ratio = Math.Max(0.0, Math.Min(1.0, ratio));

            SetProgress(ratio);
            SeekRequested?.Invoke(this, ratio);
        }

        public void SetProgress(double ratio)
        {
            progressRatio = Math.Max(0.0, Math.Min(1.0, ratio));
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            int barWidth = 3;
            int barGap = 1;
            int totalBars = this.Width / (barWidth + barGap);

            if (totalBars <= 0) return;

            int currentProgressX = (int)(this.Width * progressRatio);

            for (int i = 0; i < totalBars; i++)
            {
                int x = i * (barWidth + barGap);
                int height = 6;

                if (waveData != null && waveData.Length > 0)
                {
                    int dataIndex = (int)((double)i / totalBars * waveData.Length);
                    dataIndex = Math.Min(dataIndex, waveData.Length - 1);

                    float sample = waveData[dataIndex];
                    height = (int)(sample * (this.Height - 6));
                    height = Math.Max(6, height);
                }

                int y = (this.Height - height) / 2;

                Brush barBrush = (x <= currentProgressX) ? Brushes.OrangeRed : Brushes.Gray;
                g.FillRectangle(barBrush, x, y, barWidth, height);
            }
        }
    }
}
