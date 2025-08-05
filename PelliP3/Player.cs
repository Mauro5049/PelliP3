using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PelliP3
{
    public partial class mainWindow : Form
    {
        MusicPlayer musicPlayer = new MusicPlayer();
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
            songArtistPlayer.Text = tfile.Tag.FirstPerformer;
            songTitlePlayer.Text = tfile.Tag.Title;
            songCoverPlayer.Image = ConvertObjectToImage(tfile.Tag.Pictures[0].Data.Data);
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (musicPlayer.isMusicPlaying())
            {
                pSongButton.Text = @"|>";
                musicPlayer.stopPlaying();
            }

            else
            {
                pSongButton.Text = "||";
                musicPlayer.startPlaying();
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (songOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                loadSongInformation(songOpenFileDialog.FileName);
            }
        }
    }
}
