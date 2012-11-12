using Autofac;

using SignalR;

using TRock.Music;
using TRock.Party.Hubs;

namespace TRock.Party
{
    public class PlayerEventPublisher : IStartable
    {
        #region Fields

        private readonly ISongPlayer _player;
        private readonly IVoteableQueue<ISongStream> _queue;

        #endregion Fields

        #region Constructors

        public PlayerEventPublisher(ISongPlayer player, IVoteableQueue<ISongStream> queue)
        {
            _player = player;
            _queue = queue;
        }

        #endregion Constructors

        #region Methods

        public void Start()
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

            _player.IsPlayingChanged += (sender, args) =>
            {
                hub.Clients.isPlayingChanged(new { IsPlaying = args.NewValue, _player.CurrentSong });
            };

            _player.IsMutedChanged += (sender, args) =>
            {
                hub.Clients.isMutedChanged(args.NewValue);
            };

            _player.Progress += (sender, args) =>
            {
                hub.Clients.progress(new { args.Current, args.Total });
            };

            _player.Buffering += (sender, args) =>
            {
                hub.Clients.buffering(new { args.Current, args.Total });
            };

            _player.CurrentSongChanged += (sender, args) =>
            {
                hub.Clients.songChanged(args.NewValue);
            };

            _player.VolumeChanged += (sender, args) =>
            {
                hub.Clients.volumeChanged(args.NewValue);
            };
        }

        #endregion Methods
    }
}