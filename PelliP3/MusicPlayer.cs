using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PelliP3
{
    public class MusicPlayer
    {
        bool isPlaying = false;
        public bool isMusicPlaying()
        {
            return isPlaying;
        }
        public void startPlaying()
        {
            isPlaying = true;
        }
        public void stopPlaying()
        {
            isPlaying = false;
        }

    }
}
