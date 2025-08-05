using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;

namespace PelliP3
{
    public class MusicPlayer
    {
        bool isPlaying = false;
        IWaveSource source;
        WasapiOut soundOut = new WasapiOut();
        long songPosition = 0;
        public bool isMusicPlaying()
        {
            return isPlaying;
        }
        public IWaveSource getSource()
        {
            return source;
        }
        public void changeSong(string path)
        {
            if (isMusicPlaying())
            {
                soundOut.Stop();
                source.Dispose();
                songPosition = 0;
            }
            source = CodecFactory.Instance.GetCodec(path);
        }
        public void startPlaying()
        {
            if (source == null) return;
            isPlaying = true;
            source.Position = songPosition;
            soundOut.Initialize(source);
            soundOut.Play();
        }
        public void stopPlaying()
        {
            isPlaying = false;
            songPosition = source.Position;
            soundOut.Stop();
        }
    }
}
