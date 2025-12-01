using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using System.Text.Json;
using CSCore.SoundIn;
using System.Xml.Linq;
using System.Linq;
using static PelliP3.SongUtils;
using System.Collections.Generic;
using System.Threading.Tasks;
using PelliP3.Properties;

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

            if (Settings.Default.maxSongs != null) maxResults.Text = Settings.Default.maxSongs.ToString();

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
            if (songNameEdit.Text != string.Empty && player != null)
            {
                player.Dispose();

                uint parsedYear = 0;
                if (!uint.TryParse(yearAlbumEdit.Text, out parsedYear))
                {
                    parsedYear = 0;
                }

                saveMetadata(songNameEdit.Text, songCoverEditor.Image, artistNameEdit.Text, parsedYear);
            }
            if (maxResults.Text.Length > 0)
            {
                Properties.Settings.Default.Save();
            }
        }


        private void songCoverEditor_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            Image newCover = Image.FromFile(openFileDialog1.FileName);
            if (newCover == null) return;
            songCoverEditor.Image = newCover;
        }

        public static async Task<Song[]> RetrieveSongInformation(string songName, string artistName = null)
        {
            var results = new List<(Song song, int score)>();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");

            try
            {
                if (IsEmpty(songName))
                    return Array.Empty<Song>();

                string query = $"recording:\"{songName}\"";

                if (!IsEmpty(artistName))
                    query += $" AND artist:\"{artistName}\"";

                var fullUrl = $"https://musicbrainz.org/ws/2/recording?query={Uri.EscapeDataString(query)}&fmt=xml";
                Debug.WriteLine($"[MusicBrainzAPI] Querying: {fullUrl}");

                var response = await httpClient.GetAsync(fullUrl);
                response.EnsureSuccessStatusCode();

                var xmlContent = await response.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(xmlContent);

                var recordings = doc.Descendants().Where(e => e.Name.LocalName == "recording");

                foreach (var recording in recordings)
                {
                    int score = 0;
                    var scoreAttr = recording.Attribute("score")?.Value
                                    ?? recording.Attribute("{http://musicbrainz.org/ns/mmd-2.0#}score")?.Value;
                    if (!string.IsNullOrEmpty(scoreAttr))
                        int.TryParse(scoreAttr, out score);

                    var title = recording.Elements().FirstOrDefault(e => e.Name.LocalName == "title")?.Value?.Trim() ?? "Unknown Title";

                    var band = recording.Descendants()
                                        .FirstOrDefault(e => e.Name.LocalName == "name-credit")
                                        ?.Descendants()
                                        .FirstOrDefault(e => e.Name.LocalName == "name")
                                        ?.Value?.Trim() ?? "Unknown Artist";

                    var album = recording.Descendants()
                                         .FirstOrDefault(e => e.Name.LocalName == "release")
                                         ?.Descendants()
                                         .FirstOrDefault(e => e.Name.LocalName == "title")
                                         ?.Value?.Trim() ?? "Unknown Album";

                    var releaseDate = recording.Descendants()
                                               .FirstOrDefault(e => e.Name.LocalName == "first-release-date")
                                               ?.Value;

                    uint year = 0;
                    if (DateTime.TryParse(releaseDate, out var parsedDate))
                        year = (uint)parsedDate.Year;

                    TimeSpan duration = TimeSpan.Zero;
                    var lengthAttr = recording.Descendants()
                                              .FirstOrDefault(e => e.Name.LocalName == "length")
                                              ?.Value;
                    if (long.TryParse(lengthAttr, out var lengthMs))
                        duration = TimeSpan.FromMilliseconds(lengthMs);

                    results.Add((new Song
                    {
                        Name = title,
                        Band = band,
                        Album = album,
                        Year = year,
                        Duration = duration
                    }, score));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[MusicBrainzAPI] Error: {ex.Message}");
            }

            return results.OrderByDescending(r => r.score).Select(r => r.song).ToArray();
        }

        private static bool IsEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true;

            value = value.Trim();

            return value.Equals("Unknown", StringComparison.OrdinalIgnoreCase)
                || value.Equals("Unknown Title", StringComparison.OrdinalIgnoreCase)
                || value.Equals("Unknown Artist", StringComparison.OrdinalIgnoreCase)
                || value.Equals("Unknown Album", StringComparison.OrdinalIgnoreCase);
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            if (IsEmpty(songNameEdit.Text))
            {
                MessageBox.Show("Please enter a valid song title.");
                return;
            }

            var query = (await RetrieveSongInformation(songNameEdit.Text, artistNameEdit.Text))
                .Take(Settings.Default.maxSongs)
                .ToArray();

            if (query == null || query.Length == 0)
            {
                MessageBox.Show("No matching songs found.");
                return;
            }

            var resultForm = new SongQueryResult(query);
            if (resultForm.ShowDialog(this) == DialogResult.OK && resultForm.SelectedSong != null)
            {
                var selected = resultForm.SelectedSong;

                song.Name = selected.Name;
                song.Band = selected.Band;
                song.Album = selected.Album;
                song.Year = selected.Year;
                song.Duration = selected.Duration;

                songNameEdit.Text = song.Name;
                artistNameEdit.Text = song.Band;
                yearAlbumEdit.Text = song.Year > 0 ? song.Year.ToString() : "";

                var cover = await ArtworkUtils.RetrieveAlbumCoverAsync(selected.Name, selected.Band);
                if (cover != null)
                    songCoverEditor.Image = cover;
                else
                    Debug.WriteLine("[ArtworkAPI] No cover found.");

                MessageBox.Show("Metadata updated from MusicBrainz.");
            }
        }


        private void maxResults_TextChanged(object sender, EventArgs e)
        {
            if (maxResults.Text.Length > 0)
            {
                Settings.Default.maxSongs = int.Parse(maxResults.Text);
            }
        }
    }
}
