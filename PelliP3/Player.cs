using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using PelliP3.Properties;
using static PelliP3.SongUtils;

namespace PelliP3
{
    public partial class mainWindow : Form
    {
        private readonly List<Song> songQueue = new List<Song>(256);
        private readonly MusicPlayer musicPlayer = new MusicPlayer();
        private readonly Timer progressTimer;
        private readonly HashSet<string> loadedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> supportedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".mp3", ".wav", ".flac", ".ogg", ".aac", ".wma", ".m4a"
        };
        private Panel selectedSongPanel = null;
        private Song selectedSong;
        private String[] orderModes = { "ORDER_ASC", "ORDER_DESC", "ORDER_ALBUM" };
        private String orderMode = null;
        private const int ProgressBarResolution = 1000;
        private const string DefaultAlbumName = "Unknown Album";
        private const string DefaultArtistName = "Unknown Artist";

        private Song currentlyPlayingSong = null;

        public mainWindow()
        {
            InitializeComponent();
            songQueuePanel.AutoScroll = true;
            progressTimer = new Timer { Interval = 400 };
            progressTimer.Tick += (s, e) => progressBarChange();
            songProgressBar.Minimum = 0;
            songProgressBar.Maximum = ProgressBarResolution;
            songProgressBar.Value = 0;
            songProgressBar.MouseDown += SongProgressBar_MouseDown;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var folderPath = Properties.Settings.Default.folderScan;
            if (string.IsNullOrEmpty(folderPath))
            {
                MusicSelection musicSelection = new MusicSelection();
                musicSelection.Show();
            }
            else
            {
                loadAllSongsFromFolder();
                changeOrder_Click(sender, e);
                InitializeProgressBarSeeking();
            }
        }

        private bool addToSongQueue(Song song)
        {
            if (song?.Path == null || songQueue.Count >= 256) return false;
            if (!loadedPaths.Add(song.Path)) return false;
            songQueue.Add(song);
            return true;
        }

        private void changeSelectedSong(Song song, Panel panel)
        {
            if (song == null || panel == null) return;
            if (selectedSongPanel != null && selectedSongPanel != panel)
            {
                selectedSongPanel.BackColor = Color.Silver;
            }
            panel.BackColor = Color.FromKnownColor(KnownColor.GradientActiveCaption);
            selectedSong = song;
            selectedSongPanel = panel;
        }

        private Panel CreateSongQueueEntry(Song song)
        {
            var songEntry = new Panel
            {
                BackColor = Color.Silver,
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(391, 27),
                Tag = song
            };
            songEntry.DoubleClick += (s, e) => changeUISong(song);
            songEntry.Click += (s, e) => changeSelectedSong(song, songEntry);
            var cover = new PictureBox
            {
                Image = song.Cover ?? Resources.defaultAlbumCover,
                Location = new Point(3, 0),
                Size = new Size(27, 27),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            cover.DoubleClick += (s, e) => changeUISong(song);
            cover.Click += (s, e) => changeSelectedSong(song, songEntry);
            songEntry.Controls.Add(cover);
            var nameLabel = new Label
            {
                Text = song.Name ?? string.Empty,
                Location = new Point(36, 7),
                AutoSize = true,
                MaximumSize = new Size(290, 13)
            };
            nameLabel.DoubleClick += (s, e) => changeUISong(song);
            nameLabel.Click += (s, e) => changeSelectedSong(song, songEntry);
            songEntry.Controls.Add(nameLabel);
            var durationLabel = new Label
            {
                Text = song.Duration.ToString(@"hh\:mm\:ss"),
                Location = new Point(339, 7),
                AutoSize = true
            };
            songEntry.Controls.Add(durationLabel);
            songEntry.ContextMenuStrip = contextMenuStrip1;
            return songEntry;
        }

        private void changeUISong(Song song)
        {
            if (song == null) return;
            currentlyPlayingSong = song;
            musicPlayer.changeSong(song);
            displaySongInformation(song);
            pSongButton.Text = "|>";
            progressTimer.Stop();
            songProgressBar.Value = 0;
            currentTimeLabel.Text = "00:00:00";
        }

        private bool TryGetTimes(out TimeSpan current, out TimeSpan total)
        {
            current = TimeSpan.Zero;
            total = TimeSpan.Zero;
            var source = musicPlayer?.getSource();
            if (source == null) return false;
            try
            {
                PropertyInfo currentProp = source.GetType().GetProperty("CurrentTime");
                PropertyInfo totalProp = source.GetType().GetProperty("TotalTime");
                if (currentProp != null && totalProp != null)
                {
                    object curObj = currentProp.GetValue(source);
                    object totObj = totalProp.GetValue(source);
                    if (curObj is TimeSpan) current = (TimeSpan)curObj;
                    else if (curObj != null) current = TimeSpan.FromSeconds(Convert.ToDouble(curObj));
                    if (totObj is TimeSpan) total = (TimeSpan)totObj;
                    else if (totObj != null) total = TimeSpan.FromSeconds(Convert.ToDouble(totObj));
                    return true;
                }
                PropertyInfo wfProp = source.GetType().GetProperty("WaveFormat");
                if (wfProp != null)
                {
                    object wfObj = wfProp.GetValue(source);
                    if (wfObj != null)
                    {
                        PropertyInfo avgProp = wfObj.GetType().GetProperty("AverageBytesPerSecond");
                        if (avgProp != null)
                        {
                            int avgBytesPerSec = Convert.ToInt32(avgProp.GetValue(wfObj));
                            if (avgBytesPerSec > 0)
                            {
                                PropertyInfo posProp = source.GetType().GetProperty("Position");
                                PropertyInfo lenProp = source.GetType().GetProperty("Length");
                                if (posProp != null && lenProp != null)
                                {
                                    long pos = Convert.ToInt64(posProp.GetValue(source));
                                    long len = Convert.ToInt64(lenProp.GetValue(source));
                                    current = TimeSpan.FromSeconds(pos / (double)avgBytesPerSec);
                                    total = TimeSpan.FromSeconds(len / (double)avgBytesPerSec);
                                    return true;
                                }
                            }
                        }
                    }
                }
                if (selectedSong != null && selectedSong.Duration > TimeSpan.Zero)
                {
                    total = selectedSong.Duration;
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        private void progressBarChange()
        {
            var source = musicPlayer.getSource();
            if (source == null || currentlyPlayingSong == null) return;

            var totalSeconds = currentlyPlayingSong.Duration.TotalSeconds;
            if (totalSeconds <= 0) return;

            var bytesPerSecond = source.WaveFormat.BytesPerSecond;
            if (bytesPerSecond <= 0) return;

            var currentSeconds = source.Position / (double)bytesPerSecond;

            var progress = (int)((currentSeconds / totalSeconds) * 100);
            songProgressBar.Value = Math.Min(Math.Max(progress, 0), 100);

            currentTimeLabel.Text = TimeSpan.FromSeconds(currentSeconds).ToString(@"hh\:mm\:ss");

            if (currentSeconds >= totalSeconds)
            {
                progressTimer.Stop();
            }
        }

        private void InitializeProgressBarSeeking()
        {
            songProgressBar.Cursor = Cursors.Hand;
            songProgressBar.Click += SongProgressBar_Click;
        }

        private void SongProgressBar_Click(object sender, EventArgs e)
        {
            var source = musicPlayer.getSource();
            if (source == null || !source.CanSeek) return;

            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs == null) return;

            // Calculate the clicked position as a percentage
            var clickPosition = mouseEventArgs.X / (double)songProgressBar.Width;
            clickPosition = Math.Max(0, Math.Min(1, clickPosition)); // Clamp between 0 and 1

            // Calculate the new position in bytes
            var newPosition = (long)(source.Length * clickPosition);

            try
            {
                source.Position = newPosition;
                progressBarChange(); // Update the UI immediately
            }
            catch
            {
                // Seeking failed, ignore
            }
        }

        private void SongProgressBar_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                var source = musicPlayer?.getSource();
                if (source == null) return;
                PropertyInfo canSeekProp = source.GetType().GetProperty("CanSeek");
                if (canSeekProp != null)
                {
                    bool canSeek = Convert.ToBoolean(canSeekProp.GetValue(source));
                    if (!canSeek) return;
                }
                if (!TryGetTimes(out TimeSpan current, out TimeSpan total)) return;
                if (total.TotalSeconds <= 0) return;
                int clickX = e.X;
                int width = songProgressBar.ClientSize.Width;
                if (width <= 0) return;
                double ratio = clickX / (double)width;
                ratio = Math.Max(0.0, Math.Min(1.0, ratio));
                var target = TimeSpan.FromSeconds(ratio * total.TotalSeconds);
                SeekTo(target);
                progressBarChange();
            }
            catch
            {
            }
        }

        private void SeekTo(TimeSpan target)
        {
            var source = musicPlayer?.getSource();
            if (source == null) return;
            MethodInfo seekMethod = musicPlayer.GetType().GetMethod("Seek", new[] { typeof(TimeSpan) });
            if (seekMethod != null)
            {
                try
                {
                    seekMethod.Invoke(musicPlayer, new object[] { target });
                    return;
                }
                catch
                {
                }
            }
            try
            {
                PropertyInfo wfProp = source.GetType().GetProperty("WaveFormat");
                if (wfProp == null) return;
                object wfObj = wfProp.GetValue(source);
                if (wfObj == null) return;
                PropertyInfo avgProp = wfObj.GetType().GetProperty("AverageBytesPerSecond");
                PropertyInfo blockAlignProp = wfObj.GetType().GetProperty("BlockAlign");
                if (avgProp == null || blockAlignProp == null) return;
                int avgBytesPerSec = Convert.ToInt32(avgProp.GetValue(wfObj));
                int blockAlign = Convert.ToInt32(blockAlignProp.GetValue(wfObj));
                if (avgBytesPerSec <= 0 || blockAlign <= 0) return;
                long targetPos = (long)(target.TotalSeconds * avgBytesPerSec);
                targetPos -= (targetPos % blockAlign);
                PropertyInfo lengthProp = source.GetType().GetProperty("Length");
                if (lengthProp == null) return;
                long length = Convert.ToInt64(lengthProp.GetValue(source));
                if (targetPos < 0) targetPos = 0;
                if (targetPos > length) targetPos = length;
                PropertyInfo posProp = source.GetType().GetProperty("Position");
                if (posProp == null) return;
                posProp.SetValue(source, targetPos);
            }
            catch
            {
            }
        }

        private Song loadSongInformation(string songPath)
        {
            if (string.IsNullOrEmpty(songPath)) return null;
            try
            {
                using (var tfile = TagLib.File.Create(songPath))
                {
                    var song = new Song
                    {
                        Band = tfile.Tag.FirstPerformer ?? DefaultArtistName,
                        Name = tfile.Tag.Title ?? Path.GetFileNameWithoutExtension(songPath),
                        Album = tfile.Tag.Album ?? DefaultAlbumName,
                        Path = songPath,
                        Duration = tfile.Properties?.Duration ?? TimeSpan.Zero,
                        Year = tfile.Tag.Year,
                        File = tfile
                    };
                    if (tfile.Tag.Pictures?.Length > 0)
                    {
                        var pictureData = tfile.Tag.Pictures[0].Data?.Data;
                        song.Cover = WindowUtils.ConvertBytesToImage(pictureData);
                    }
                    return song;
                }
            }
            catch
            {
                return null;
            }
        }

        private void loadAllSongsFromFolder()
        {
            songQueue.Clear();
            loadedPaths.Clear();
            var folderPath = Properties.Settings.Default.folderScan;
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath)) return;
            try
            {
                var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                                     .Where(f => supportedExtensions.Contains(Path.GetExtension(f)));
                songQueuePanel.AutoScrollPosition = new Point(0, 0);
                songQueuePanel.SuspendLayout();
                foreach (var file in files)
                {
                    var song = loadSongInformation(file);
                    if (song != null)
                    {
                        addToSongQueue(song);
                    }
                }
                refreshSongPlaylist();
                songQueuePanel.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading songs: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void displaySongInformation(Song song)
        {
            if (song == null) return;
            songArtistPlayer.Text = song.Band;
            songCoverPlayer.Image = song.Cover ?? Resources.defaultAlbumCover;
            songTitlePlayer.Text = song.Name;
        }

        private void refreshSongPlaylist()
        {
            songQueuePanel.Controls.Clear();
            const int yStart = 14;
            const int yIncrement = 35;
            var yOffset = yStart;
            foreach (var song in songQueue)
            {
                if (song == null) continue;
                var entry = CreateSongQueueEntry(song);
                entry.Location = new Point(14, yOffset);
                songQueuePanel.Controls.Add(entry);
                yOffset += yIncrement;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (musicPlayer.getSource() == null) return;
            if (musicPlayer.isMusicPlaying())
            {
                pSongButton.Text = "|>";
                musicPlayer.stopPlaying();
                progressTimer.Stop();
            }
            else
            {
                pSongButton.Text = "||";
                musicPlayer.startPlaying();
                progressTimer.Start();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (songOpenFileDialog.ShowDialog() != DialogResult.OK) return;
            var song = loadSongInformation(songOpenFileDialog.FileName);
            if (song == null) return;
            if (addToSongQueue(song))
            {
                musicPlayer.changeSong(song);
                displaySongInformation(song);
                refreshSongPlaylist();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (contextMenuStrip1.SourceControl.Tag is Song)
            {
                Metadata metadata = new Metadata();
                metadata.player = musicPlayer;
                metadata.Tag = contextMenuStrip1.SourceControl.Tag;
                metadata.Show();
                metadata.FormClosed += (s, ev) => loadAllSongsFromFolder();
            }
        }

        private void diToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MusicSelection musicSelection = new MusicSelection();
            musicSelection.Show();
            musicSelection.Closing += (s, ev) => loadAllSongsFromFolder();
        }

        private void changeOrder_Click(object sender, EventArgs e)
        {
            int currentIndex = Array.IndexOf(orderModes, orderMode);
            int nextIndex = (currentIndex < 0) ? 0 : (currentIndex + 1) % orderModes.Length;
            orderMode = orderModes[nextIndex];
            Comparison<Song> compareByName = (a, b) =>
            {
                if (a == null && b == null) return 0;
                if (a == null) return -1;
                if (b == null) return 1;
                return string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
            };
            switch (orderMode)
            {
                case "ORDER_ASC":
                    songQueue.Sort(compareByName);
                    changeOrder.Text = "A → Z";
                    break;
                case "ORDER_DESC":
                    songQueue.Sort((a, b) => -compareByName(a, b));
                    changeOrder.Text = "Z → A";
                    break;
                case "ORDER_ALBUM":
                    songQueue.Sort((a, b) =>
                    {
                        if (a == null && b == null) return 0;
                        if (a == null) return -1;
                        if (b == null) return 1;
                        int albumCompare = string.Compare(a.Album ?? DefaultAlbumName, b.Album ?? DefaultAlbumName, StringComparison.OrdinalIgnoreCase);
                        if (albumCompare != 0) return albumCompare;
                        return string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
                    });
                    changeOrder.Text = "By Album";
                    break;
                default:
                    break;
            }
            Song previouslySelected = selectedSong;
            selectedSongPanel = null;
            refreshSongPlaylist();
            if (previouslySelected != null)
            {
                foreach (Control ctl in songQueuePanel.Controls)
                {
                    if (ctl is Panel p && p.Tag is Song s && string.Equals(s.Path, previouslySelected.Path, StringComparison.OrdinalIgnoreCase))
                    {
                        changeSelectedSong(s, p);
                        break;
                    }
                }
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}