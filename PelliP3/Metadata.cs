using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PelliP3
{
    public partial class Metadata : Form
    {
        SongUtils.Song song;
        public Metadata()
        {
            InitializeComponent();
        }

        private void saveMetadata(string name, Image image, string artistName, uint year)
        {
            song.Name = name;
            if (image != Properties.Resources.defaultAlbumCover)
                song.Cover = image;
            if (artistName != String.Empty)
                song.Band = artistName;
            if (year != null)
                song.Year = year;
            
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
            if (songNameEdit.Text != String.Empty)
            {
                saveMetadata(songNameEdit.Text, songCoverEditor.Image, artistNameEdit.Text, uint.Parse(yearAlbumEdit.Text));
            }
        }
    }
}
