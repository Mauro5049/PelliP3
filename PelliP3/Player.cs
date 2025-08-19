using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PelliP3.Properties;
using static PelliP3.SongUtils;

namespace PelliP3
{
    public partial class mainWindow : Form
    {
        MusicPlayer musicPlayer = new MusicPlayer();
        string defaultArtistName = String.Empty;
        string defaultAlbumName = String.Empty;
        SongUtils.Song[] songQueue = new SongUtils.Song[256];

        private void addToSongQueue(SongUtils.Song[] songQueue, SongUtils.Song song)
        {
            int index = 0;
            for (int i = 0; i < songQueue.Length; i++)
            {
                if (songQueue[i] == null) { index = i; break; }
                if (songQueue[i].Path == song.Path) { return; }
            }
            songQueue[index] = song;
        }
       private Panel CreateSongQueueEntry(SongUtils.Song song)
        {
            Panel songEntry = new Panel();
            songEntry.BackColor = Color.Silver;
            songEntry.BorderStyle = BorderStyle.FixedSingle;
            songEntry.Size = new Size(391, 27);
            songEntry.Click += (sender, e) => changeUISong(song);

            PictureBox cover = new PictureBox();
            cover.Image = song.Cover;
            cover.Location = new Point(3, 0);
            cover.Size = new Size(27, 27);
            cover.SizeMode = PictureBoxSizeMode.StretchImage;
            songEntry.Controls.Add(cover);
            cover.Click += (sender, e) => changeUISong(song);

            Label nameLabel = new Label();
            nameLabel.Text = song.Name;
            nameLabel.Location = new Point(36, 7);
            songEntry.Controls.Add(nameLabel);
            nameLabel.Click += (sender, e) => changeUISong(song);

            Label durationLabel = new Label();
            durationLabel.Text = song.Duration.ToString(@"hh\:mm\:ss");
            durationLabel.Location = new Point(339, 7);
            songEntry.Controls.Add(durationLabel);

            return songEntry;
        }

        private void changeUISong(SongUtils.Song song)
        {
            musicPlayer.changeSong(song);
            displaySongInformation(song);
            pSongButton.Text = @"|>";
        }

        public static Image ConvertObjectToImage(object obj)
        {
            if (obj is Image img)
            {
                return img;
            }
            else if (obj is byte[] bytes)
            {
                using (var ms = new System.IO.MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }
            else
            {
                Debug.WriteLine($"{obj.GetType()}");
                throw new InvalidCastException("Object cannot be converted to Image.");
            }
        }
        private SongUtils.Song loadSongInformation(string songName)
        {
            SongUtils.Song song = new SongUtils.Song();
            var tfile = TagLib.File.Create(songName);
            if (tfile.Tag.FirstPerformer != null) song.Band = tfile.Tag.FirstPerformer;
            else song.Band = defaultArtistName;
            if (tfile.Tag.Title != null) song.Name = tfile.Tag.Title;
            else song.Name = tfile.Name;
            if (tfile.Tag.Pictures.Length > 0) song.Cover = ConvertObjectToImage(tfile.Tag.Pictures[0].Data.Data);
            else song.Cover = Resources.defaultAlbumCover;
            song.Album = tfile.Tag.Album;
            song.Path = songName;
            return song;
        }
        private void displaySongInformation(SongUtils.Song song)
        {
            songArtistPlayer.Text = song.Band;
            songCoverPlayer.Image = song.Cover;
            songTitlePlayer.Text = song.Name;
        }
        private void refreshSongPlaylist(SongUtils.Song[] queue)
        {
            int yOffset = 14;
            for (int i = 0; i < queue.Length; i++)
            {
                if (queue[i] == null) continue;
                Panel entry = CreateSongQueueEntry(queue[i]);
                entry.Location = new Point(14, yOffset);
                songQueuePanel.Controls.Add(entry);

                yOffset += 35;
            }
            GC.Collect();

        }
        public mainWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            Debug.WriteLine("TODO");
        }

        private void button3_Click(object sender, EventArgs e)
        {
           if (musicPlayer.getSource() == null) return;
            if (musicPlayer.isMusicPlaying())
            {
                pSongButton.Text = @"|>";
                musicPlayer.stopPlaying();
            }
            else
            { 
                pSongButton.Text = "||";
                musicPlayer.startPlaying();
                progressBarChange();
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (songOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                var surface_strait = loadSongInformation(songOpenFileDialog.FileName);
                addToSongQueue(songQueue, surface_strait);
                musicPlayer.changeSong(surface_strait);
                displaySongInformation(surface_strait);
                refreshSongPlaylist(songQueue);
            }
        }
    }
}
