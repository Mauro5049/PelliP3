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

namespace PelliP3
{
    public partial class mainWindow : Form
    {
        MusicPlayer musicPlayer = new MusicPlayer();
        string defaultArtistName = String.Empty;
        string defaultAlbumName = String.Empty;
        string[] songQueue = new string[256];

        private void addToSongQueue(string[] songQueue, string songPath)
        {
            int index = 0;
            for (int i = 0; i < songQueue.Length; i++)
            {
                if (songQueue[i] == songPath) { return; }
                if (songQueue[i]  != null) { index = i; break; }
            }
            songQueue[index] = songPath;
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
        private void loadSongInformation(string songName)
        {
            var tfile = TagLib.File.Create(songName);
            if (tfile.Tag.FirstPerformer != null) songArtistPlayer.Text = tfile.Tag.FirstPerformer;
            else songArtistPlayer.Text = defaultArtistName;
            if (tfile.Tag.Title != null) songTitlePlayer.Text = tfile.Tag.Title;
            else songTitlePlayer.Text = tfile.Name;
            if (tfile.Tag.Pictures.Length > 0) songCoverPlayer.Image = ConvertObjectToImage(tfile.Tag.Pictures[0].Data.Data);
            else songCoverPlayer.Image = Resources.defaultAlbumCover;
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
                loadSongInformation(songOpenFileDialog.FileName);
                musicPlayer.changeSong(songOpenFileDialog.FileName);
                addToSongQueue(songQueue, songOpenFileDialog.FileName);
                Debug.WriteLine(songQueue);
            }
        }
    }
}
