using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PelliP3
{
    public static class ArtworkUtils
    {
        public static async Task<Image> RetrieveAlbumCoverAsync(string songName, string artistName)
        {
            if (string.IsNullOrWhiteSpace(songName) || string.IsNullOrWhiteSpace(artistName))
                return null;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; PelliP3/1.0)");

            try
            {
                string baseUrl = "https://itunes.apple.com/search";
                string query = $"?term={Uri.EscapeDataString(songName)}&artist={Uri.EscapeDataString(artistName)}&media=music&limit=1";
                string fullUrl = baseUrl + query;

                Debug.WriteLine($"[ArtworkAPI] Querying: {fullUrl}");

                var response = await httpClient.GetAsync(fullUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);

                var root = doc.RootElement;
                if (!root.TryGetProperty("resultCount", out var countProp) || countProp.GetInt32() == 0)
                    return null;

                var results = root.GetProperty("results");
                if (results.GetArrayLength() == 0)
                    return null;

                var first = results[0];
                if (!first.TryGetProperty("artworkUrl100", out var artProp))
                    return null;

                string artworkUrl = artProp.GetString();
                if (string.IsNullOrWhiteSpace(artworkUrl))
                    return null;

                // Replace resolution pattern (e.g. 100x100bb) with 3000x3000bb
                string highResUrl = System.Text.RegularExpressions.Regex.Replace(
                    artworkUrl,
                    @"\/\d+x\d+bb\.jpg",
                    "/3000x3000bb.jpg",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase
                );

                Debug.WriteLine($"[ArtworkAPI] Downloading: {highResUrl}");

                var imageBytes = await httpClient.GetByteArrayAsync(highResUrl);
                var ms = new MemoryStream(imageBytes);
                return Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ArtworkAPI] Error: {ex.Message}");
                return null;
            }
        }
    }
}
