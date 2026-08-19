#nullable disable

using DJing; // WaveformControl 클래스가 포함된 네임스페이스
using MySql.Data.MySqlClient;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DJing
{
    public partial class soundcloud : Form
    {
        private string dbHost = "127.0.0.1";
        private string dbPort = "3306";
        private string dbUser = "root";
        private string dbPass = "1111"; // <--- 본인 MySQL 비밀번호 입력
        private string dbName = "djing";

        private Panel topPanel;
        private Button btnMyPage;
        private FlowLayoutPanel songListPanel;

        private TextBox txtSearch;
        private Button btnSearch;

        // 하단 컨트롤바 UI 구성 요소
        public Panel bottomPlayerPanel;
        private PictureBox picBottomAlbum;
        private Label lblBottomTitle;
        private Label lblBottomArtist;
        private Button btnBottomPrev;
        private Button btnBottomPlay;
        private Button btnBottomNext;
        private WaveformControl bottomWaveControl;
        private Label lblCurrentTime;
        private Label lblTotalTime;
        private TrackBar trackVolume;
        private Label lblVolumeIcon;

        // 오디오 재생 관련 변수
        private WaveOutEvent globalWaveOut;
        private AudioFileReader globalAudioFile;
        private System.Windows.Forms.Timer playerTimer;
        private List<SongData> currentSongList = new List<SongData>();
        private int currentSongIndex = -1;
        private int currentPlayingSongId = -1;
        private bool isUserStopping = false; // 사용자 의도적 정지 구분을 위한 플래그
        private Mypage? myPage;

        public soundcloud()
        {
            InitializeComponent();
            InitializeFormUI();
            InitPlayerTimer();
            LoadSongsToUI();
        }

        private void InitializeFormUI()
        {
            this.Text = "SoundCloud Player";
            this.Size = new Size(920, 720);
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 1. 상단 패널
            topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(25, 25, 25),
                Padding = new Padding(10)
            };
            topPanel.Resize += TopPanel_Resize;

            btnMyPage = new Button
            {
                Text = "마이페이지",
                Size = new Size(120, 36),
                Location = new Point(775, 12),
                Font = new Font("맑은 고딕", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(255, 85, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnMyPage.FlatAppearance.BorderSize = 0;
            btnMyPage.Click += BtnMyPage_Click;

            txtSearch = new TextBox
            {
                Size = new Size(290, 28),
                Font = new Font("맑은 고딕", 10),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.KeyDown += TxtSearch_KeyDown;

            btnSearch = new Button
            {
                Text = "검색",
                Size = new Size(65, 28),
                Font = new Font("맑은 고딕", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 50, 50),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;

            topPanel.Controls.Add(btnMyPage);
            topPanel.Controls.Add(txtSearch);
            topPanel.Controls.Add(btnSearch);

            CenterSearchControls();

            // 2. 하단 컨트롤바 패널
            InitializeBottomPlayerPanel();

            // 3. 중앙 스크롤 곡 리스트 패널
            songListPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.None,

                Location = new Point(0, topPanel.Height),

                Size = new Size(ClientSize.Width, ClientSize.Height - topPanel.Height - bottomPlayerPanel.Height),

                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Bottom |
                    AnchorStyles.Left |
                    AnchorStyles.Right,

                AutoSize = false,
                AutoScroll = true,
                BackColor = Color.FromArgb(18, 18, 18),
                Padding = new Padding(15, 10, 0, 25),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            this.Controls.Add(songListPanel);
            this.Controls.Add(topPanel);
            this.Controls.Add(bottomPlayerPanel);
        }

        private void InitializeBottomPlayerPanel()
        {
            bottomPlayerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                BackColor = Color.FromArgb(28, 28, 28)
            };

            // 앨범 이미지
            picBottomAlbum = new PictureBox
            {
                Size = new Size(56, 56),
                Location = new Point(12, 12),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(45, 45, 45)
            };

            // 곡 정보
            lblBottomTitle = new Label
            {
                Text = "선택된 곡 없음",
                Location = new Point(75, 18),
                Size = new Size(100, 20),
                Font = new Font("맑은 고딕", 9.5f, FontStyle.Bold),
                ForeColor = Color.White
            };

            lblBottomArtist = new Label
            {
                Text = "-",
                Location = new Point(75, 40),
                Size = new Size(100, 18),
                Font = new Font("맑은 고딕", 8.5f, FontStyle.Regular),
                ForeColor = Color.DarkGray
            };

            // 이전/재생/다음 버튼
            btnBottomPrev = new Button
            {
                Text = "|◀",
                Size = new Size(32, 32),
                Location = new Point(180, 24),
                Font = new Font("맑은 고딕", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBottomPrev.FlatAppearance.BorderSize = 0;
            btnBottomPrev.Click += (s, e) => PlayPreviousSong();

            btnBottomPlay = new Button
            {
                Text = "▶",
                Size = new Size(38, 38),
                Location = new Point(215, 21),
                Font = new Font("맑은 고딕", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(255, 85, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBottomPlay.FlatAppearance.BorderSize = 0;
            btnBottomPlay.Click += BtnBottomPlay_Click;

            btnBottomNext = new Button
            {
                Text = "▶|",
                Size = new Size(32, 32),
                Location = new Point(256, 24),
                Font = new Font("맑은 고딕", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBottomNext.FlatAppearance.BorderSize = 0;
            btnBottomNext.Click += (s, e) => PlayNextSong();

            // 시간 및 하단 웨이브바
            lblCurrentTime = new Label
            {
                Text = "0:00",
                Location = new Point(295, 32),
                Size = new Size(35, 18),
                Font = new Font("맑은 고딕", 8f),
                ForeColor = Color.LightGray,
                TextAlign = ContentAlignment.MiddleRight
            };

            bottomWaveControl = new WaveformControl
            {
                Location = new Point(335, 20),
                Size = new Size(345, 40),
                BackColor = Color.FromArgb(38, 38, 38),
                Cursor = Cursors.Hand
            };
            bottomWaveControl.SeekRequested += (s, targetRatio) => SeekAudioToRatio(targetRatio);

            lblTotalTime = new Label
            {
                Text = "0:00",
                Location = new Point(685, 32),
                Size = new Size(35, 18),
                Font = new Font("맑은 고딕", 8f),
                ForeColor = Color.LightGray,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // 볼륨 컨트롤
            lblVolumeIcon = new Label
            {
                Text = "VOL",
                Location = new Point(735, 31),
                Size = new Size(30, 20),
                Font = new Font("맑은 고딕", 8f, FontStyle.Bold),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter
            };

            trackVolume = new TrackBar
            {
                Location = new Point(768, 25),
                Size = new Size(110, 30),
                Minimum = 0,
                Maximum = 100,
                Value = 80,
                TickStyle = TickStyle.None
            };
            trackVolume.ValueChanged += TrackVolume_ValueChanged;

            bottomPlayerPanel.Controls.Add(picBottomAlbum);
            bottomPlayerPanel.Controls.Add(lblBottomTitle);
            bottomPlayerPanel.Controls.Add(lblBottomArtist);
            bottomPlayerPanel.Controls.Add(btnBottomPrev);
            bottomPlayerPanel.Controls.Add(btnBottomPlay);
            bottomPlayerPanel.Controls.Add(btnBottomNext);
            bottomPlayerPanel.Controls.Add(lblCurrentTime);
            bottomPlayerPanel.Controls.Add(bottomWaveControl);
            bottomPlayerPanel.Controls.Add(lblTotalTime);
            bottomPlayerPanel.Controls.Add(lblVolumeIcon);
            bottomPlayerPanel.Controls.Add(trackVolume);
        }

        private void InitPlayerTimer()
        {
            playerTimer = new System.Windows.Forms.Timer { Interval = 100 };
            playerTimer.Tick += PlayerTimer_Tick;
        }

        private void SeekAudioToRatio(double targetRatio)
        {
            if (globalAudioFile == null) return;

            long targetPosition = (long)(globalAudioFile.Length * targetRatio);

            int align = globalAudioFile.WaveFormat.BlockAlign;
            if (align > 0)
            {
                targetPosition = (targetPosition / align) * align;
            }

            targetPosition = Math.Max(0, Math.Min(globalAudioFile.Length, targetPosition));
            globalAudioFile.Position = targetPosition;

            TimeSpan current = globalAudioFile.CurrentTime;
            lblCurrentTime.Text = FormatTime(current);

            bottomWaveControl.SetProgress(targetRatio);
            UpdateItemControlWaveforms();
        }

        private void CenterSearchControls()
        {
            if (txtSearch == null || btnSearch == null || topPanel == null) return;

            int spacing = 8;
            int totalWidth = txtSearch.Width + spacing + btnSearch.Width;
            int startX = (topPanel.Width - totalWidth) / 2 + 20;

            txtSearch.Location = new Point(startX, 16);
            btnSearch.Location = new Point(startX + txtSearch.Width + spacing, 16);
        }

        private void TopPanel_Resize(object sender, EventArgs e)
        {
            CenterSearchControls();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadSongsToUI(txtSearch.Text.Trim());
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadSongsToUI(txtSearch.Text.Trim());
        }

        private void BtnMyPage_Click(object sender, EventArgs e)
        {
            if (myPage == null || myPage.IsDisposed)
            {
                myPage = new Mypage(this);
            }

            myPage.StartPosition =
                FormStartPosition.Manual;

            myPage.Location = Location;

            myPage.AttachBottomPlayer();

            Hide();

            if (!myPage.Visible)
            {
                myPage.Show(this);
            }

            myPage.BringToFront();
            myPage.Activate();
        }

        private void BtnBottomPlay_Click(object sender, EventArgs e)
        {
            if (globalWaveOut == null || globalAudioFile == null)
            {
                if (currentSongList.Count > 0)
                {
                    PlaySongAtIndex(0);
                }

                return;
            }

            TogglePlayPause();
        }

        private string GetConnectionString(bool includeDb = true)
        {
            string connStr = $"Server={dbHost};Port={dbPort};Uid={dbUser};Pwd={dbPass};CharSet=utf8mb4;";
            if (includeDb)
            {
                connStr += $"Database={dbName};";
            }
            return connStr;
        }

        private List<SongData> FetchSongsFromDB(string keyword = "")
        {
            var list = new List<SongData>();
            try
            {
                using (var conn = new MySqlConnection(GetConnectionString(true)))
                {
                    conn.Open();

                    string query = "SELECT SongId, Title, Artist, FilePath, ImagePath, LikeCount, ReleaseDate FROM songs ";
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        query += "WHERE Title LIKE @keyword OR Artist LIKE @keyword ";
                    }
                    query += "ORDER BY SongId DESC;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(keyword))
                        {
                            cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new SongData
                                {
                                    SongId = reader.GetInt32("SongId"),
                                    Title = reader.GetString("Title"),
                                    Artist = reader.GetString("Artist"),
                                    FilePath = reader.IsDBNull(reader.GetOrdinal("FilePath")) ? "" : reader.GetString("FilePath"),
                                    ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? "" : reader.GetString("ImagePath"),
                                    LikeCount = reader.IsDBNull(reader.GetOrdinal("LikeCount")) ? 0 : reader.GetInt32("LikeCount"),
                                    ReleaseDate = reader.IsDBNull(reader.GetOrdinal("ReleaseDate")) ? "" : reader.GetString("ReleaseDate")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"데이터 조회 실패:\n{ex.Message}", "DB 오류");
            }
            return list;
        }

        // ★ [개선] 순차 스트리밍 버퍼 독출 방식으로 파형 계산식 전면 개편
        private float[] ExtractWaveformData(string filePath, int pointCount = 100)
        {
            float[] rawAverages = new float[pointCount];
            float[] finalPeaks = new float[pointCount];

            // 기본 예비값 (0.15 높이)
            for (int i = 0; i < pointCount; i++) finalPeaks[i] = 0.15f;

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return finalPeaks;
            }

            try
            {
                using (var reader = new AudioFileReader(filePath))
                {
                    long totalSamples = reader.Length / (reader.WaveFormat.BitsPerSample / 8);
                    long samplesPerPoint = totalSamples / pointCount;
                    if (samplesPerPoint <= 0) samplesPerPoint = 1;

                    float[] readBuffer = new float[8192];
                    int currentPoint = 0;
                    long samplesInCurrentPoint = 0;
                    double currentSum = 0;

                    int read;
                    while ((read = reader.Read(readBuffer, 0, readBuffer.Length)) > 0)
                    {
                        for (int i = 0; i < read; i++)
                        {
                            // 단순 Max 대신 절대값의 누적합(평균 음량) 계산
                            currentSum += Math.Abs(readBuffer[i]);
                            samplesInCurrentPoint++;

                            if (samplesInCurrentPoint >= samplesPerPoint)
                            {
                                if (currentPoint < pointCount)
                                {
                                    rawAverages[currentPoint] = (float)(currentSum / samplesInCurrentPoint);
                                    currentPoint++;
                                }
                                samplesInCurrentPoint = 0;
                                currentSum = 0;
                            }
                        }
                    }

                    if (currentPoint < pointCount && samplesInCurrentPoint > 0)
                    {
                        rawAverages[currentPoint] = (float)(currentSum / samplesInCurrentPoint);
                    }

                    // 곡 전체 내에서의 최소/최대 평균값 탐색 (상대 정규화 기준값)
                    float maxVal = 0.0001f;
                    float minVal = float.MaxValue;
                    for (int i = 0; i < pointCount; i++)
                    {
                        if (rawAverages[i] > maxVal) maxVal = rawAverages[i];
                        if (rawAverages[i] < minVal) minVal = rawAverages[i];
                    }

                    // 가장 작은 구간과 큰 구간을 0.15 ~ 0.95 비율 사이로 정규화 (파형 굴곡 극대화)
                    for (int i = 0; i < pointCount; i++)
                    {
                        float normalized = (rawAverages[i] - minVal) / (maxVal - minVal + 0.0001f);
                        finalPeaks[i] = 0.15f + (normalized * 0.80f);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[파형 추출 예외] {ex.Message}");
            }

            return finalPeaks;
        }

        public void LoadSongsToUI(string keyword = "")
        {
            songListPanel.Controls.Clear();
            currentSongList = FetchSongsFromDB(keyword);

            for (int i = 0; i < currentSongList.Count; i++)
            {
                int index = i;
                var song = currentSongList[i];
                var itemControl = new SongItemControl(GetConnectionString(true));
                itemControl.BindData(song);

                // 실제 음원 파일에서 파형 추출 및 적용
                float[] peaks = ExtractWaveformData(song.FilePath, 100);
                itemControl.SetWaveformData(peaks);

                itemControl.PlayRequested += (s, songData) =>
                {
                    bool isCurrentSong = 
                        currentPlayingSongId == 
                        songData.SongId;

                    if (isCurrentSong &&
                        globalWaveOut != null)
                    {
                        TogglePlayPause();
                    }
                    else
                    {
                        int selectedIndex =
                            currentSongList.FindIndex(
                                x => x.SongId ==
                                     songData.SongId);

                        if (selectedIndex >= 0)
                        {
                            PlaySongAtIndex(selectedIndex);
                        }
                    }
                };

                itemControl.SeekRequested += (s, targetRatio) =>
                {
                    if (currentSongIndex != index)
                    {
                        PlaySongAtIndex(index);
                    }
                    SeekAudioToRatio(targetRatio);
                };

                songListPanel.Controls.Add(itemControl);
            }
            UpdateItemControlWaveforms();
        }

        public void PlaySongById(int songId)
        {
            // 현재 메인폼이 가지고 있는 곡 목록에서 SongId 검색
            int index = currentSongList.FindIndex(song => song.SongId == songId);

            // 현재 목록에 곡이 없다면 전체 곡을 DB에서 다시 가져옴
            if (index == -1)
            {
                currentSongList = FetchSongsFromDB();

                index = currentSongList.FindIndex(song => song.SongId == songId);
            }

            // 그래도 못 찾았다면 종료
            if (index == -1)
            {
                MessageBox.Show("재생할 곡을 찾을 수 없습니다.");
                return;
            }

            // 기존 메인폼 재생 기능 사용
            PlaySongAtIndex(index);
        }

        public void PlaySongAtIndex(int index)
        {
            if (currentSongList == null || currentSongList.Count == 0) return;
            if (index < 0 || index >= currentSongList.Count) return;

            SongData song = currentSongList[index];

            currentSongIndex = index;
            currentPlayingSongId = song.SongId;

            if (string.IsNullOrEmpty(song.FilePath) || !File.Exists(song.FilePath))
            {
                MessageBox.Show($"음원 파일을 찾을 수 없습니다:\n{song.FilePath}", "파일 오류");
                return;
            }

            try
            {
                StopGlobalAudio();

                currentSongIndex = index;

                globalAudioFile = new AudioFileReader(song.FilePath);
                globalAudioFile.Volume = trackVolume.Value / 100f;

                globalWaveOut = new WaveOutEvent();
                globalWaveOut.Init(globalAudioFile);
                globalWaveOut.PlaybackStopped += GlobalWaveOut_PlaybackStopped;

                globalWaveOut.Play();
                playerTimer.Start();

                // UI 업데이트
                lblBottomTitle.Text = song.Title;
                lblBottomArtist.Text = song.Artist;
                btnBottomPlay.Text = "||";

                lblTotalTime.Text = FormatTime(globalAudioFile.TotalTime);

                // 하단 재생바 파형 업데이트
                float[] currentPeaks = ExtractWaveformData(song.FilePath, 100);
                bottomWaveControl.SetWaveformData(currentPeaks);
                bottomWaveControl.SetProgress(0);

                if (picBottomAlbum.Image != null)
                {
                    var oldImg = picBottomAlbum.Image;
                    picBottomAlbum.Image = null;
                    oldImg.Dispose();
                }

                if (!string.IsNullOrEmpty(song.ImagePath) && File.Exists(song.ImagePath))
                {
                    using (var stream = new FileStream(song.ImagePath, FileMode.Open, FileAccess.Read))
                    {
                        picBottomAlbum.Image = new Bitmap(stream);
                    }
                }

                UpdateItemControlWaveforms();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"재생 중 오류 발생: {ex.Message}");
            }
        }

        public bool TogglePlayPause()
        {
            if (globalWaveOut == null || globalAudioFile == null)
                return false;

            if (globalWaveOut.PlaybackState == PlaybackState.Playing)
            {
                globalWaveOut.Pause();
                playerTimer.Stop();
                btnBottomPlay.Text = "▶";

                UpdateItemControlWaveforms();

                return false;
            }
            else if (globalWaveOut.PlaybackState == PlaybackState.Paused)
            {
                globalWaveOut.Play();
                playerTimer.Start();
                btnBottomPlay.Text = "||";

                UpdateItemControlWaveforms();

                return true;
            }

            return false;
        }

        private void PlayNextSong()
        {
            if (currentSongList == null || currentSongList.Count == 0) return;

            int nextIndex = currentSongIndex + 1;
            if (nextIndex >= currentSongList.Count)
            {
                nextIndex = 0;
            }
            PlaySongAtIndex(nextIndex);
        }

        private void PlayPreviousSong()
        {
            if (currentSongList == null || currentSongList.Count == 0) return;

            int prevIndex = currentSongIndex - 1;
            if (prevIndex < 0)
            {
                prevIndex = currentSongList.Count - 1;
            }
            PlaySongAtIndex(prevIndex);
        }

        private void GlobalWaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => GlobalWaveOut_PlaybackStopped(sender, e)));
                return;
            }

            playerTimer.Stop();

            if (!isUserStopping && globalAudioFile != null && globalAudioFile.Position >= globalAudioFile.Length)
            {
                PlayNextSong();
            }
            else
            {
                btnBottomPlay.Text = "▶";
                if (globalAudioFile != null)
                {
                    globalAudioFile.Position = 0;
                }
                lblCurrentTime.Text = "0:00";
                bottomWaveControl.SetProgress(0);
                UpdateItemControlWaveforms();
            }
        }

        private void PlayerTimer_Tick(object sender, EventArgs e)
        {
            if (globalAudioFile != null && globalAudioFile.Length > 0)
            {
                TimeSpan current = globalAudioFile.CurrentTime;
                lblCurrentTime.Text = FormatTime(current);

                double progress = (double)globalAudioFile.Position / globalAudioFile.Length;
                bottomWaveControl.SetProgress(progress);

                UpdateItemControlWaveforms();
            }
        }

        private void TrackVolume_ValueChanged(object sender, EventArgs e)
        {
            if (globalAudioFile != null)
            {
                globalAudioFile.Volume = trackVolume.Value / 100f;
            }
        }

        private void UpdateItemControlWaveforms()
        {
            foreach (Control control
                     in songListPanel.Controls)
            {
                if (control is SongItemControl item)
                {
                    bool isCurrentSong =
                        item.SongId ==
                        currentPlayingSongId;

                    bool isPlaying =
                        isCurrentSong &&
                        globalWaveOut?.PlaybackState ==
                            PlaybackState.Playing;

                    double progress = 0;

                    if (isCurrentSong &&
                        globalAudioFile != null &&
                        globalAudioFile.Length > 0)
                    {
                        progress =
                            (double)globalAudioFile.Position /
                            globalAudioFile.Length;
                    }

                    item.UpdateWaveState(
                        isPlaying,
                        progress
                    );
                }
            }
        }

        private void StopGlobalAudio()
        {
            isUserStopping = true;
            playerTimer.Stop();

            if (globalWaveOut != null)
            {
                globalWaveOut.PlaybackStopped -= GlobalWaveOut_PlaybackStopped;
                globalWaveOut.Stop();
                globalWaveOut.Dispose();
                globalWaveOut = null;
            }
            if (globalAudioFile != null)
            {
                globalAudioFile.Dispose();
                globalAudioFile = null;
            }
            isUserStopping = false;
        }

        private string FormatTime(TimeSpan ts)
        {
            return $"{(int)ts.TotalMinutes}:{ts.Seconds:D2}";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopGlobalAudio();
            base.OnFormClosing(e);
        }

        public void RestorePlayerAndReload()
        {
            SuspendLayout();

            if (bottomPlayerPanel.Parent != this)
            {
                Controls.Add(bottomPlayerPanel);
            }

            bottomPlayerPanel.Dock = DockStyle.Bottom;
            bottomPlayerPanel.BringToFront();

            songListPanel.Dock = DockStyle.Fill;
            songListPanel.AutoScroll = false;
            songListPanel.AutoScrollPosition = Point.Empty;

            ResumeLayout(true);
            PerformLayout();

            BeginInvoke(new Action(() =>
            {
                songListPanel.AutoScroll = true;
                LoadSongsToUI();

                songListPanel.AutoScrollPosition =
                    Point.Empty;

                songListPanel.PerformLayout();
                Refresh();
            }));
        }

        private void soundcloud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(UserSession.UserId == "Guest")
                Application.Exit();
        }
    }
}