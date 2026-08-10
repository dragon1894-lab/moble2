using System.Drawing.Drawing2D;
using System.Media;

namespace DDD;

public sealed class GameForm : Form
{
    private enum GameState { Ready, Playing, Paused, GameOver }
    private enum TargetKind { Mole, Gold, Bomb }
    private sealed record Particle(float X, float Y, float Dx, float Dy, int Life, Color Color, string Text);

    private readonly System.Windows.Forms.Timer loop = new() { Interval = 16 };
    private readonly Random random = new();
    private readonly List<Particle> particles = [];
    private GameState state = GameState.Ready;
    private TargetKind targetKind;
    private int activeHole = -1, previousHole = -1;
    private int score, highScore, combo, bestCombo, lives, level;
    private float timeLeft, targetTime, targetDuration, shake;
    private DateTime lastTick;
    private readonly RectangleF[] holes = new RectangleF[9];
    private readonly Font titleFont = new("Segoe UI", 30, FontStyle.Bold);
    private readonly Font hudFont = new("Segoe UI", 14, FontStyle.Bold);
    private readonly Font normalFont = new("Segoe UI", 12, FontStyle.Bold);
    private readonly Font bigFont = new("Segoe UI", 23, FontStyle.Bold);

    public GameForm()
    {
        Text = "두더지 대소동!";
        ClientSize = new Size(820, 720);
        MinimumSize = new Size(720, 650);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(246, 239, 211);
        DoubleBuffered = true;
        KeyPreview = true;
        SetStyle(ControlStyles.ResizeRedraw, true);
        loop.Tick += TickGame;
        MouseDown += HitTarget;
        KeyDown += HandleKey;
        lastTick = DateTime.UtcNow;
        loop.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        if (shake > 0) g.TranslateTransform(random.Next(-4, 5), random.Next(-4, 5));
        DrawBackground(g);
        LayoutHoles();
        DrawHud(g);
        for (int i = 0; i < holes.Length; i++) DrawHole(g, holes[i], i);
        DrawParticles(g);
        if (state != GameState.Playing) DrawOverlay(g);
    }

    private void DrawBackground(Graphics g)
    {
        using var sky = new LinearGradientBrush(ClientRectangle, Color.FromArgb(151, 222, 255), Color.FromArgb(240, 249, 200), 90);
        g.FillRectangle(sky, ClientRectangle);
        using var sun = new SolidBrush(Color.FromArgb(255, 229, 93));
        g.FillEllipse(sun, Width - 130, 28, 72, 72);
        using var hill1 = new SolidBrush(Color.FromArgb(127, 195, 91));
        using var hill2 = new SolidBrush(Color.FromArgb(93, 169, 75));
        g.FillEllipse(hill1, -180, 130, Width * .8f, 320);
        g.FillEllipse(hill2, Width * .35f, 125, Width * .85f, 340);
        using var grass = new SolidBrush(Color.FromArgb(106, 181, 70));
        g.FillRectangle(grass, 0, 245, Width, Height - 245);
    }

    private void LayoutHoles()
    {
        float boardW = Math.Min(700, Width - 70), gapX = 24, gapY = 34;
        float w = (boardW - gapX * 2) / 3, h = Math.Min(120, (Height - 340 - gapY * 2) / 3f);
        float startX = (Width - boardW) / 2, startY = 280;
        for (int r = 0; r < 3; r++) for (int c = 0; c < 3; c++)
            holes[r * 3 + c] = new RectangleF(startX + c * (w + gapX), startY + r * (h + gapY), w, h);
    }

    private void DrawHud(Graphics g)
    {
        using var white = new SolidBrush(Color.White);
        using var dark = new SolidBrush(Color.FromArgb(45, 65, 50));
        using var card = new SolidBrush(Color.FromArgb(220, 255, 255, 255));
        using var titleShadow = new SolidBrush(Color.FromArgb(70, 0, 0, 0));
        g.DrawString("두더지 대소동!", titleFont, titleShadow, 24, 25);
        g.DrawString("두더지 대소동!", titleFont, white, 21, 22);
        var hud = new RectangleF(28, 105, Width - 56, 118);
        FillRound(g, card, hud, 22);
        g.DrawString($"점수  {score:N0}", hudFont, dark, 50, 122);
        g.DrawString($"최고  {highScore:N0}", normalFont, dark, 50, 163);
        g.DrawString($"레벨  {level}", hudFont, dark, Width / 2 - 47, 122);
        g.DrawString(combo >= 2 ? $"🔥 콤보 x{combo}" : "콤보를 이어가세요!", normalFont, dark, Width / 2 - 78, 163);
        g.DrawString($"목숨  {new string('♥', Math.Max(0, lives))}", hudFont, Brushes.Crimson, Width - 245, 122);
        float ratio = Math.Clamp(timeLeft / 45f, 0, 1);
        var bar = new RectangleF(Width - 245, 170, 185, 18);
        FillRound(g, Brushes.WhiteSmoke, bar, 9);
        var barColor = ratio < .25f ? Color.OrangeRed : Color.FromArgb(44, 190, 125);
        using var fill = new SolidBrush(barColor);
        FillRound(g, fill, new RectangleF(bar.X, bar.Y, bar.Width * ratio, bar.Height), 9);
        g.DrawString($"{Math.Ceiling(timeLeft):00}초", normalFont, dark, Width - 150, 192, new StringFormat { Alignment = StringAlignment.Center });
    }

    private void DrawHole(Graphics g, RectangleF r, int index)
    {
        var hole = new RectangleF(r.X + 8, r.Bottom - 43, r.Width - 16, 47);
        using var holeBrush = new LinearGradientBrush(hole, Color.FromArgb(45, 34, 25), Color.FromArgb(108, 73, 40), 90);
        g.FillEllipse(holeBrush, hole);
        if (index == activeHole && state == GameState.Playing)
        {
            float p = Math.Clamp(targetTime / .18f, 0, 1);
            if (targetTime > targetDuration - .2f) p = Math.Clamp((targetDuration - targetTime) / .2f, 0, 1);
            DrawTarget(g, r, p, targetKind);
        }
        using var rim = new Pen(Color.FromArgb(82, 117, 48), 8);
        g.DrawArc(rim, hole, 5, 170);
    }

    private void DrawTarget(Graphics g, RectangleF r, float pop, TargetKind kind)
    {
        float size = Math.Min(112, r.Width - 28), x = r.X + (r.Width - size) / 2;
        float y = r.Bottom - 38 - size * pop;
        var body = new RectangleF(x, y, size, size * 1.08f);
        var main = kind == TargetKind.Gold ? Color.FromArgb(255, 196, 42) : kind == TargetKind.Bomb ? Color.FromArgb(55, 59, 66) : Color.FromArgb(139, 88, 50);
        using var bodyBrush = new SolidBrush(main);
        g.FillEllipse(bodyBrush, body);
        if (kind == TargetKind.Bomb)
        {
            using var fuse = new Pen(Color.SaddleBrown, 6);
            g.DrawArc(fuse, x + size * .64f, y - 14, 35, 35, 190, 100);
            g.FillEllipse(Brushes.OrangeRed, x + size * .9f, y - 10, 12, 12);
            using var markFont = new Font("Segoe UI", 33, FontStyle.Bold);
            g.DrawString("!", markFont, Brushes.White, x + size * .38f, y + size * .25f);
            return;
        }
        using var earBrush = new SolidBrush(main);
        g.FillEllipse(earBrush, x + 6, y + 14, 30, 36);
        g.FillEllipse(earBrush, x + size - 36, y + 14, 30, 36);
        using var muzzle = new SolidBrush(kind == TargetKind.Gold ? Color.FromArgb(255, 231, 133) : Color.FromArgb(219, 171, 117));
        g.FillEllipse(muzzle, x + size * .21f, y + size * .42f, size * .58f, size * .43f);
        g.FillEllipse(Brushes.White, x + size * .23f, y + size * .28f, 24, 28);
        g.FillEllipse(Brushes.White, x + size * .58f, y + size * .28f, 24, 28);
        g.FillEllipse(Brushes.Black, x + size * .30f, y + size * .36f, 8, 11);
        g.FillEllipse(Brushes.Black, x + size * .65f, y + size * .36f, 8, 11);
        g.FillEllipse(Brushes.DarkSlateGray, x + size * .43f, y + size * .52f, 18, 14);
        using var smile = new Pen(Color.FromArgb(70, 40, 25), 3);
        g.DrawArc(smile, x + size * .34f, y + size * .58f, size * .32f, 23, 10, 160);
        if (kind == TargetKind.Gold)
        {
            using var starFont = new Font("Segoe UI Symbol", 18, FontStyle.Bold);
            g.DrawString("★", starFont, Brushes.White, x + size * .39f, y + 2);
        }
    }

    private void DrawParticles(Graphics g)
    {
        foreach (var p in particles)
        {
            using var b = new SolidBrush(Color.FromArgb(Math.Clamp(p.Life * 8, 0, 255), p.Color));
            g.DrawString(p.Text, normalFont, b, p.X, p.Y);
        }
    }

    private void DrawOverlay(Graphics g)
    {
        using var dim = new SolidBrush(Color.FromArgb(175, 20, 33, 31));
        g.FillRectangle(dim, ClientRectangle);
        string title = state switch { GameState.Ready => "준비됐나요?", GameState.Paused => "잠깐 쉬는 중", _ => "게임 종료!" };
        string detail = state switch
        {
            GameState.Ready => "두더지를 클릭하고 폭탄은 피하세요!\n황금 두더지는 5배 점수!",
            GameState.Paused => "P 키를 누르면 계속합니다",
            _ => $"최종 점수  {score:N0}\n최고 콤보  x{bestCombo}"
        };
        var center = new StringFormat { Alignment = StringAlignment.Center };
        g.DrawString(title, bigFont, Brushes.White, Width / 2, Height / 2 - 105, center);
        g.DrawString(detail, normalFont, Brushes.WhiteSmoke, Width / 2, Height / 2 - 47, center);
        var button = StartButton;
        FillRound(g, Brushes.Gold, button, 24);
        string buttonText = state == GameState.Paused ? "계속하기" : state == GameState.GameOver ? "다시 도전!" : "게임 시작!";
        g.DrawString(buttonText, hudFont, Brushes.DarkSlateGray, button.X + button.Width / 2, button.Y + 15, center);
        g.DrawString("SPACE: 시작   P: 일시정지   ESC: 종료", normalFont, Brushes.WhiteSmoke, Width / 2, button.Bottom + 28, center);
    }

    private RectangleF StartButton => new(Width / 2f - 115, Height / 2f + 35, 230, 60);

    private void TickGame(object? sender, EventArgs e)
    {
        var now = DateTime.UtcNow;
        float dt = (float)Math.Min(.05, (now - lastTick).TotalSeconds);
        lastTick = now;
        if (state == GameState.Playing)
        {
            timeLeft -= dt;
            targetTime -= dt;
            if (targetTime <= 0) MissTarget();
            if (timeLeft <= 0 || lives <= 0) EndGame();
        }
        for (int i = particles.Count - 1; i >= 0; i--)
        {
            var p = particles[i];
            particles[i] = p with { X = p.X + p.Dx, Y = p.Y + p.Dy, Dy = p.Dy + .08f, Life = p.Life - 1 };
            if (particles[i].Life <= 0) particles.RemoveAt(i);
        }
        shake = Math.Max(0, shake - dt);
        Invalidate();
    }

    private void NewGame()
    {
        score = combo = bestCombo = 0;
        lives = 3;
        level = 1;
        timeLeft = 45;
        particles.Clear();
        state = GameState.Playing;
        SpawnTarget();
    }

    private void SpawnTarget()
    {
        level = Math.Min(10, 1 + score / 700);
        do activeHole = random.Next(9); while (activeHole == previousHole);
        previousHole = activeHole;
        int roll = random.Next(100);
        targetKind = roll < Math.Min(22, 7 + level) ? TargetKind.Bomb : roll < 30 ? TargetKind.Gold : TargetKind.Mole;
        targetDuration = Math.Max(.42f, 1.15f - level * .065f) + (float)random.NextDouble() * .22f;
        targetTime = targetDuration;
    }

    private void HitTarget(object? sender, MouseEventArgs e)
    {
        if (state != GameState.Playing)
        {
            if (StartButton.Contains(e.Location)) ToggleStart();
            return;
        }
        if (activeHole < 0 || !holes[activeHole].Contains(e.Location)) return;
        var center = new PointF(holes[activeHole].X + holes[activeHole].Width / 2, holes[activeHole].Y + 30);
        if (targetKind == TargetKind.Bomb)
        {
            lives--;
            combo = 0;
            score = Math.Max(0, score - 150);
            shake = .35f;
            Burst(center, Color.OrangeRed, "-150");
            SystemSounds.Hand.Play();
        }
        else
        {
            combo++;
            bestCombo = Math.Max(bestCombo, combo);
            int basePoints = targetKind == TargetKind.Gold ? 500 : 100;
            int gained = basePoints + Math.Min(400, combo * 15);
            score += gained;
            timeLeft = Math.Min(45, timeLeft + (targetKind == TargetKind.Gold ? 1.2f : .25f));
            Burst(center, targetKind == TargetKind.Gold ? Color.Gold : Color.White, $"+{gained}");
            SystemSounds.Asterisk.Play();
        }
        SpawnTarget();
    }

    private void MissTarget()
    {
        if (targetKind != TargetKind.Bomb)
        {
            combo = 0;
            lives--;
            Burst(new PointF(holes[activeHole].X + 45, holes[activeHole].Y), Color.White, "놓쳤다!");
        }
        SpawnTarget();
    }

    private void Burst(PointF p, Color color, string text)
    {
        particles.Add(new Particle(p.X - 25, p.Y, 0, -1.6f, 32, color, text));
        for (int i = 0; i < 7; i++)
            particles.Add(new Particle(p.X, p.Y, (float)(random.NextDouble() * 5 - 2.5), (float)(-random.NextDouble() * 3), 25, color, "●"));
    }

    private void EndGame()
    {
        state = GameState.GameOver;
        activeHole = -1;
        highScore = Math.Max(highScore, score);
        SystemSounds.Exclamation.Play();
    }

    private void HandleKey(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape) Close();
        else if (e.KeyCode == Keys.Space && state != GameState.Playing) ToggleStart();
        else if (e.KeyCode == Keys.P && state is GameState.Playing or GameState.Paused)
        {
            state = state == GameState.Playing ? GameState.Paused : GameState.Playing;
            lastTick = DateTime.UtcNow;
        }
    }

    private void ToggleStart()
    {
        if (state == GameState.Paused) { state = GameState.Playing; lastTick = DateTime.UtcNow; }
        else NewGame();
    }

    private static void FillRound(Graphics g, Brush brush, RectangleF r, float radius)
    {
        if (r.Width <= 0 || r.Height <= 0) return;
        float d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
        using var path = new GraphicsPath();
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        g.FillPath(brush, path);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) { loop.Dispose(); titleFont.Dispose(); hudFont.Dispose(); normalFont.Dispose(); bigFont.Dispose(); }
        base.Dispose(disposing);
    }
}
