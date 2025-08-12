using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PelliP3
{
    public class SongUtils
    {
        public class Song
        {
            public string Name { get; set; }
            public string Album { get; set; }
            public string Band { get; set; }
            public TimeSpan Duration { get; set; }
            public Image Cover { get; set; }
            public string Path { get; set; }
        }

      /*  public void createSong(string name, TimeSpan duration, Image cover, string path)
        {
            Song song = new Song();
            song.Name = name;
            song.Duration = duration;
            song.Cover = cover;
            song.Path = path;
        }
      */
    }
}
