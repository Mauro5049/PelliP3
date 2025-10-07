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

        public static async Task<Song[]> RetrieveSongInformation(string songName, string artistName = null)
        {
            var results = new List<(Song song, int score)>(); // keep score for sorting

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");

            try
            {
                // Build the query URL
                var baseUrl = "https://musicbrainz.org/ws/2/recording";
                var query = $"?query={Uri.EscapeDataString(songName)}";

                if (!string.IsNullOrEmpty(artistName))
                {
                    query += "&artist=" + Uri.EscapeDataString(artistName);
                }

                var fullUrl = baseUrl + query;
                Debug.WriteLine($"Querying: {fullUrl}");

                var response = await httpClient.GetAsync(fullUrl);
                response.EnsureSuccessStatusCode();

                var xmlContent = await response.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(xmlContent);

                // Get all <recording> elements (ignore namespaces)
                var recordings = doc.Descendants().Where(e => e.Name.LocalName == "recording");

                foreach (var recording in recordings)
                {
                    // Parse score
                    int score = 0;
                    var scoreAttr = recording.Attribute("score")?.Value
                                    ?? recording.Attribute("{http://musicbrainz.org/ns/mmd-2.0#}score")?.Value;
                    if (!string.IsNullOrEmpty(scoreAttr))
                        int.TryParse(scoreAttr, out score);

                    if (score < 90)
                        continue;

                    // Title
                    var title = recording.Descendants()
                                         .FirstOrDefault(e => e.Name.LocalName == "title")
                                         ?.Value ?? "Unknown";

                    // Artist/Band
                    var band = recording.Descendants()
                                        .FirstOrDefault(e => e.Name.LocalName == "name-credit")
                                        ?.Descendants()
                                        .FirstOrDefault(e => e.Name.LocalName == "name")
                                        ?.Value ?? "Unknown";

                    // Album
                    var album = recording.Descendants()
                                         .FirstOrDefault(e => e.Name.LocalName == "release")
                                         ?.Descendants()
                                         .FirstOrDefault(e => e.Name.LocalName == "title")
                                         ?.Value ?? "Unknown";

                    // Release date
                    var releaseDate = recording.Descendants()
                                               .FirstOrDefault(e => e.Name.LocalName == "first-release-date")
                                               ?.Value;

                    uint year = 0;
                    if (DateTime.TryParse(releaseDate, out var parsedDate))
                        year = (uint)parsedDate.Year;

                    // Duration
                    TimeSpan duration = TimeSpan.Zero;
                    var lengthAttr = recording.Descendants()
                                              .FirstOrDefault(e => e.Name.LocalName == "length")
                                              ?.Value;
                    if (long.TryParse(lengthAttr, out var lengthMs))
                        duration = TimeSpan.FromMilliseconds(lengthMs);

                    var song = new Song
                    {
                        Name = title,
                        Band = band,
                        Album = album,
                        Year = year,
                        Duration = duration
                    };

                    results.Add((song, score));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[MusicBrainzAPI] Error: {ex.Message}");
            }

            // Return results ordered by score descending
            return results.OrderByDescending(r => r.score).Select(r => r.song).ToArray();
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            if (songNameEdit == null)
            {
                MessageBox.Show("Song name can't be null.");
                return;
            }
            var weirdFishes = await RetrieveSongInformation(songNameEdit.Text, string.IsNullOrWhiteSpace(artistNameEdit.Text) ? null : artistNameEdit.Text);

            Debug.WriteLine("I love coding more than my wife");

            if (weirdFishes == null || weirdFishes.Length == 0) Debug.Write("you're all I need."); return;

            // TODO: FIx this non-working pile of shit.

            Debug.WriteLine("I wish I had a wife.");

            foreach (var song in weirdFishes)
            {
                Debug.WriteLine(song.Name);
            }

            Debug.WriteLine("Then maybe I'd love her more than coding.");
        }
    }
}
