using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;

namespace PelliP3
{
    public static class SongUtils
    {
        public class Song : IEquatable<Song>
        {
            private string _name;
            private string _album;
            private string _band;

            public string Name
            {
                get => _name ?? string.Empty;
                set => _name = value;
            }

            public string Album
            {
                get => _album ?? string.Empty;
                set => _album = value;
            }

            public string Band
            {
                get => _band ?? string.Empty;
                set => _band = value;
            }

            public TimeSpan Duration { get; set; }
            public Image Cover { get; set; }
            public string Path { get; set; }
            public uint Year { get; set; }
            
            public TagLib.File File { get; set; }

            public bool Equals(Song other)
            {
                if (other == null) return false;
                return string.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Song);
            }

            public override int GetHashCode()
            {
                return Path?.GetHashCode() ?? 0;
            }
        }
    }
}