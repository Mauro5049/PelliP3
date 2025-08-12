using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PelliP3
{
    internal class SongUtils
    {
        public class Song
        {
            public string Name { get; set; }
            public TimeSpan Duration { get; set; }
            public Image Cover { get; set; }
            public String Path { get; set; }
        }

    }
}
