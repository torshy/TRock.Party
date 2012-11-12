using System;
using System.Diagnostics;
using System.Web.Http;
using TRock.Music;

namespace TRock.Party.Controllers
{
    public class PlayerController : ApiController
    {
        #region Fields

        private readonly ISongPlayer _songPlayer;

        #endregion Fields

        #region Constructors

        public PlayerController(ISongPlayer songPlayer)
        {
            _songPlayer = songPlayer;
        }

        #endregion Constructors

        #region Methods

        public void Play()
        {
            try
            {
                _songPlayer.Play();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        public void Stop()
        {
            try
            {
                _songPlayer.Stop();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        public void Pause()
        {
            try
            {
                _songPlayer.Pause();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        public void VolumeUp()
        {
            try
            {
                _songPlayer.Volume += 0.1f;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        public void VolumeDown()
        {
            try
            {
                _songPlayer.Volume -= 0.1f;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        public void ToggleMute()
        {
            try
            {
                _songPlayer.IsMuted = !_songPlayer.IsMuted;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        public void TogglePlay()
        {
            try
            {
                _songPlayer.IsPlaying = !_songPlayer.IsPlaying;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        #endregion Methods
    }
}
