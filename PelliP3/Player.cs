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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static PelliP3.SongUtils;

namespace PelliP3
{
    public partial class mainWindow : Form
    {
        private readonly List<SongUtils.Song> songQueue = new List<SongUtils.Song>(256);
        private readonly MusicPlayer musicPlayer = new MusicPlayer();

        private Metadata metadata = new Metadata();
        private MusicSelection musicSelection = new MusicSelection();

        private Panel selectedSongPanel = null;

        private SongUtils.Song selectedSong;

        private string defaultAlbumName = string.Empty;
        private string defaultArtistName = string.Empty;

        private Timer progressTimer;

        private void addToSongQueue(SongUtils.Song song)
        {
            if (song == null || string.IsNullOrEmpty(song.Path)) return;
            if (songQueue.Any(s => string.Equals(s?.Path, song.Path, StringComparison.OrdinalIgnoreCase))) return;
            if (songQueue.Count >= 256) return;
            songQueue.Add(song);
        }

        private void changeSelectedSong(SongUtils.Song song, Panel panel)
        {
            if (song == null || panel == null) return;

            if (selectedSongPanel != null && selectedSongPanel != panel)
            {
                selectedSongPanel.BackColor = Color.Silver;
            }

            panel.BackColor = Color.FromName("GradientActiveCaption");

            selectedSong = song;
            selectedSongPanel = panel;
        }

        private Panel CreateSongQueueEntry(SongUtils.Song song)
        {
            var songEntry = new Panel
            {
                BackColor = Color.Silver,
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(391, 27),
                Tag = song
            };

            songEntry.DoubleClick += (sender, e) => changeUISong(song);
            songEntry.Click += (sender, e) => changeSelectedSong(song, songEntry);

            var cover = new PictureBox
            {
                Image = song?.Cover ?? Resources.defaultAlbumCover,
                Location = new Point(3, 0),
                Size = new Size(27, 27),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            songEntry.Controls.Add(cover);
            cover.Click += (sender, e) => changeUISong(song);
            cover.Click += (sender, e) => changeSelectedSong(song, songEntry);

            var nameLabel = new Label
            {
                Text = song?.Name ?? string.Empty,
                Location = new Point(36, 7),
                AutoSize = true
            };
            songEntry.Controls.Add(nameLabel);
            nameLabel.Click += (sender, e) => changeUISong(song);
            nameLabel.Click += (sender, e) => changeSelectedSong(song, songEntry);

            var durationLabel = new Label
            {
                Text = (song?.Duration ?? TimeSpan.Zero).ToString(@"hh\:mm\:ss"),
                Location = new Point(339, 7),
                AutoSize = true
            };
            songEntry.Controls.Add(durationLabel);

            return songEntry;
        }

        private void changeUISong(SongUtils.Song song)
        {
            if (song == null) return;
            musicPlayer.changeSong(song);
            displaySongInformation(song);
            pSongButton.Text = "|>";
            progressTimer?.Stop();
            songProgressBar.Value = 0;
        }

        public static Image ConvertObjectToImage(object obj)
        {
            if (obj is Image img)
            {
                return img;
            }

            if (obj is byte[] bytes && bytes.Length > 0)
            {
                using (var ms = new System.IO.MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }

            throw new InvalidCastException("Object cannot be converted to Image.");
        }

        private SongUtils.Song loadSongInformation(string songName)
        {
            if (string.IsNullOrEmpty(songName)) return null;

            var song = new SongUtils.Song();

            try
            {
                using (var tfile = TagLib.File.Create(songName))
                {
                    song.Band = tfile.Tag.FirstPerformer ?? defaultArtistName;
                    song.Name = tfile.Tag.Title ?? tfile.Name ?? string.Empty;
                    song.Album = tfile.Tag.Album ?? defaultAlbumName;
                    song.Path = songName;
                    song.Duration = tfile.Properties?.Duration ?? TimeSpan.Zero;

                    if (tfile.Tag.Pictures != null && tfile.Tag.Pictures.Length > 0)
                    {
                        song.Cover = ConvertObjectToImage(tfile.Tag.Pictures[0].Data.Data);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return song;
        }

        private void loadAllSongsFromFolder()
        {
            var folderPath = Properties.Settings.Default.folderScan;
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath)) return;

            var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".mp3",
                ".wav",
                ".flac",
                ".ogg",
                ".aac",
                ".wma",
                ".m4a"
            };

            try
            {
                var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                                     .Where(f => extensions.Contains(Path.GetExtension(f)));

                foreach (var file in files)
                {
                    var song = loadSongInformation(file);
                    if (song != null)
                    {
                        addToSongQueue(song);
                    }
                }

                refreshSongPlaylist();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading songs: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void displaySongInformation(SongUtils.Song song)
        {
            if (song == null) return;
            songArtistPlayer.Text = song.Band ?? string.Empty;
            songCoverPlayer.Image = song.Cover ?? Resources.defaultAlbumCover;
            songTitlePlayer.Text = song.Name ?? string.Empty;
        }

        private void refreshSongPlaylist()
        {
            songQueuePanel.Controls.Clear();
            var yOffset = 14;
            foreach (var s in songQueue)
            {
                if (s == null) continue;
                var entry = CreateSongQueueEntry(s);
                entry.Location = new Point(14, yOffset);
                songQueuePanel.Controls.Add(entry);
                yOffset += 35;
            }
        }

        public mainWindow()
        {
            InitializeComponent();
            songQueuePanel.AutoScroll = true;
            progressTimer = new Timer { Interval = 1000 };
            progressTimer.Tick += (s, e) => progressBarChange();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.folderScan))
            {
                musicSelection.Show();
            }
            else
            {
                loadAllSongsFromFolder();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void progressBarChange()
        {
            var source = musicPlayer.getSource();
            if (source == null) return;

            var secs = source.Position / (double)source.WaveFormat.SampleRate;
            var maxSecs = source.Length / (double)source.WaveFormat.SampleRate;
            if (maxSecs <= 0) return;

            var progress = (int)((secs / maxSecs) * 100);
            songProgressBar.Value = Math.Min(Math.Max(progress, 0), 100);
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

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (songOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                var surface_strait = loadSongInformation(songOpenFileDialog.FileName);
                if (surface_strait == null) return;
                addToSongQueue(surface_strait);
                musicPlayer.changeSong(surface_strait);
                displaySongInformation(surface_strait);
                refreshSongPlaylist();
            }
        }

        private void editMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metadata.ShowDialog();
        }
    }
}
