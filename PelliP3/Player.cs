using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private Metadata metadata = new Metadata();
        private MusicSelection musicSelection = new MusicSelection();
        private Panel selectedSongPanel = null;
        private Song selectedSong;

        private const string DefaultAlbumName = "Unknown Album";
        private const string DefaultArtistName = "Unknown Artist";

        public mainWindow()
        {
            InitializeComponent();
            songQueuePanel.AutoScroll = true;
            progressTimer = new Timer { Interval = 1000 };
            progressTimer.Tick += (s, e) => progressBarChange();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var folderPath = Properties.Settings.Default.folderScan;
            if (string.IsNullOrEmpty(folderPath))
            {
                musicSelection.Show();
            }
            else
            {
                loadAllSongsFromFolder();
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

            EventHandler clickHandler = (s, e) =>
            {
                changeUISong(song);
                changeSelectedSong(song, songEntry);
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
            cover.Click += clickHandler;
            songEntry.Controls.Add(cover);

            var nameLabel = new Label
            {
                Text = song.Name ?? string.Empty,
                Location = new Point(36, 7),
                AutoSize = true,
                MaximumSize = new Size(290, 13)
            };
            nameLabel.Click += clickHandler;
            songEntry.Controls.Add(nameLabel);

            var durationLabel = new Label
            {
                Text = song.Duration.ToString(@"hh\:mm\:ss"),
                Location = new Point(339, 7),
                AutoSize = true
            };
            songEntry.Controls.Add(durationLabel);

            return songEntry;
        }

        private void changeUISong(Song song)
        {
            if (song == null) return;

            musicPlayer.changeSong(song);
            displaySongInformation(song);
            pSongButton.Text = "|>";
            progressTimer.Stop();
            songProgressBar.Value = 0;
            currentTimeLabel.Text = "00:00:00";
        }

        private static Image ConvertBytesToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;

            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
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
                        Duration = tfile.Properties?.Duration ?? TimeSpan.Zero
                    };

                    if (tfile.Tag.Pictures?.Length > 0)
                    {
                        var pictureData = tfile.Tag.Pictures[0].Data?.Data;
                        song.Cover = ConvertBytesToImage(pictureData);
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
            var folderPath = Properties.Settings.Default.folderScan;
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath)) return;

            try
            {
                var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                                     .Where(f => supportedExtensions.Contains(Path.GetExtension(f)));

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

        private void progressBarChange()
        {
            var source = musicPlayer.getSource();
            if (source == null) return;

            var sampleRate = source.WaveFormat.SampleRate;
            if (sampleRate <= 0) return;

            var currentSeconds = source.Position / (double)sampleRate;
            var totalSeconds = source.Length / (double)sampleRate;

            if (totalSeconds <= 0) return;

            var progress = (int)((currentSeconds / totalSeconds) * 100);
            songProgressBar.Value = Math.Min(Math.Max(progress, 0), 100);

            currentTimeLabel.Text = TimeSpan.FromSeconds(currentSeconds).ToString(@"hh\:mm\:ss");
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

        private void editMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metadata.ShowDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
        }
    }
}