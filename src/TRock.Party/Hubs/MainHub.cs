using System.Collections.Generic;

using SignalR;
using SignalR.Hubs;

using TRock.Music;
using TRock.Party.Controllers;

namespace TRock.Party.Hubs
{
    public class MainHub : Hub
    {
        #region Fields

        private readonly IVoteableQueue<ISongStream> _queue;
        private readonly ISongPlayer _songPlayer;

        #endregion Fields

        #region Constructors

        public MainHub()
        {
            _songPlayer = GlobalHost.DependencyResolver.Resolve<ISongPlayer>();
            _queue = GlobalHost.DependencyResolver.Resolve<IVoteableQueue<ISongStream>>();
        }

        #endregion Constructors

        #region Methods

        public dynamic IsPlaying()
        {
            return new
            {
                _songPlayer.IsPlaying,
                _songPlayer.CurrentSong
            };
        }

        public bool IsMuted()
        {
            return _songPlayer.IsMuted;
        }

        public Song CurrentSong()
        {
            return _songPlayer.CurrentSong;
        }

        public IEnumerable<dynamic> GetQueue()
        {
            return QueueController.ConvertToWebType(_queue.CurrentQueue);
        }

        #endregion Methods
    }
}