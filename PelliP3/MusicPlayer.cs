using System;
using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;

namespace PelliP3
{
    public class MusicPlayer : IDisposable
    {
        private readonly WasapiOut soundOut = new WasapiOut();
        private IWaveSource source;
        private long songPosition;
        private bool isPlaying;
        private bool isInitialized;

        public bool isMusicPlaying() => isPlaying;

        public IWaveSource getSource() => source;

        public void changeSong(SongUtils.Song song)
        {
            if (song?.Path == null) return;

            if (isPlaying)
            {
                stopPlaying();
            }

            source?.Dispose();
            songPosition = 0;
            isInitialized = false;

            try
            {
                source = CodecFactory.Instance.GetCodec(song.Path);
            }
            catch
            {
                source = null;
            }
        }

        public void startPlaying()
        {
            if (source == null) return;

            try
            {
                source.Position = songPosition;

                if (!isInitialized)
                {
                    soundOut.Initialize(source);
                    isInitialized = true;
                }

                soundOut.Play();
                isPlaying = true;
            }
            catch
            {
                isPlaying = false;
                isInitialized = false;
            }
        }

        public void stopPlaying()
        {
            if (!isPlaying) return;

            try
            {
                if (source != null)
                {
                    songPosition = source.Position;
                }

                soundOut.Stop();
                isPlaying = false;
            }
            catch
            {
                isPlaying = false;
            }
        }

        public void Dispose()
        {
            stopPlaying();
            source?.Dispose();
            soundOut?.Dispose();
        }
    }
}