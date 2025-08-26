using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PelliP3.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static PelliP3.SongUtils;

namespace PelliP3
{
    public partial class mainWindow : Form
    {
        private readonly MusicPlayer musicPlayer = new MusicPlayer();
        private readonly List<SongUtils.Song> songQueue = new List<SongUtils.Song>(256);
        private string defaultArtistName = string.Empty;
        private string defaultAlbumName = string.Empty;
        private Timer progressTimer;
        Metadata metadata;

        private void addToSongQueue(SongUtils.Song song)
        {
            if (song == null || string.IsNullOrEmpty(song.Path)) return;
            if (songQueue.Any(s => string.Equals(s?.Path, song.Path, StringComparison.OrdinalIgnoreCase))) return;
            if (songQueue.Count >= 256) return;
            songQueue.Add(song);
        }

        private Panel CreateSongQueueEntry(SongUtils.Song song)
        {
            var songEntry = new Panel
            {
                BackColor = Color.Silver,
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(391, 27)
            };
            songEntry.Click += (sender, e) => changeUISong(song);

            var cover = new PictureBox
            {
                Image = song?.Cover ?? Resources.defaultAlbumCover,
                Location = new Point(3, 0),
                Size = new Size(27, 27),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            songEntry.Controls.Add(cover);
            cover.Click += (sender, e) => changeUISong(song);

            var nameLabel = new Label
            {
                Text = song?.Name ?? string.Empty,
                Location = new Point(36, 7),
                AutoSize = true
            };
            songEntry.Controls.Add(nameLabel);
            nameLabel.Click += (sender, e) => changeUISong(song);

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
            progressTimer.Stop();
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
            return song;
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
            int yOffset = 14;
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
            progressTimer = new Timer { Interval = 1000 };
            progressTimer.Tick += (s, e) => progressBarChange();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            metadata = new Metadata();
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

            double secs = source.Position / source.WaveFormat.SampleRate;
            double maxSecs = source.Length / source.WaveFormat.SampleRate;

            int progress = (int)((secs / maxSecs) * 100);
            songProgressBar.Value = Math.Min(progress, 100);
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
