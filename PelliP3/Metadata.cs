using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using System.Text.Json;
using CSCore.SoundIn;
using System.Xml.Linq;
using System.Linq;

namespace PelliP3
{
    public partial class Metadata : Form
    {
        SongUtils.Song song;

        public MusicPlayer player;

        public Metadata()
        {
            InitializeComponent();
        }

        private void saveMetadata(string name, Image image, string artistName, uint year)
        {
            song.File.Tag.Title = name;
            if (image != Properties.Resources.defaultAlbumCover)
                song.File.Tag.Pictures = new TagLib.IPicture[] { WindowUtils.ConvertImageToTagLibPicture(image) };
            if (artistName != String.Empty)
                if (song.File.Tag.Performers.Length > 0) song.File.Tag.Performers[0] = artistName;
                else song.File.Tag.Performers = new[] { artistName };
            if (year != null)
                song.File.Tag.Year = year;
            song.File.Save();
            MessageBox.Show("Saved!");
        }

        private void Metadata_Load(object sender, EventArgs e)
        {
            if (Tag.GetType() != typeof(SongUtils.Song))
                Close();
            else
                song = (SongUtils.Song)Tag;

            songCoverEditor.Image = song.Cover ?? Properties.Resources.defaultAlbumCover;
            songNameEdit.Text = song.Name;
            artistNameEdit.Text = song.Band;
            yearAlbumEdit.Text = song.Year.ToString();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (songNameEdit.Text != String.Empty && player != null)
            {
                player.Dispose();
                saveMetadata(songNameEdit.Text, songCoverEditor.Image, artistNameEdit.Text, uint.Parse(yearAlbumEdit.Text));
            }
        }

        private void songCoverEditor_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            Image newCover = Image.FromFile(openFileDialog1.FileName);
            if (newCover == null) return;
            songCoverEditor.Image = newCover;
        }
        public async void RetrieveSongInformation(String name)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
            try
            {
                var response = await httpClient.GetAsync($"https://musicbrainz.org/ws/2/recording?query={Uri.EscapeDataString(name)}");
                response.EnsureSuccessStatusCode();
                var crazycattle3d = await response.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(crazycattle3d);

                var ns = (XNamespace)"http://musicbrainz.org/ns/mmd-2.0#";

                // men shaking hands
              
            }
            catch (HttpRequestException ex)
            {
                Debug.Write(ex.Message);
                Debug.Write("Its been a hard day's night");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (songNameEdit == null)
            {
                MessageBox.Show("Song name can't be null.");
                return;
            }
            RetrieveSongInformation(songNameEdit.Text);
        }
    }
}
