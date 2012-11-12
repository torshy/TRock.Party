using System.Threading;

using Autofac;

using TRock.Music;

namespace TRock.Party
{
    public class SongStreamHandler : IStartable
    {
        #region Fields

        private readonly IVoteableQueue<ISongStream> _queue;
        private readonly ISongPlayer _songPlayer;
        private readonly ISongStreamPlayer _streamPlayer;

        #endregion Fields

        #region Constructors

        public SongStreamHandler(
            IVoteableQueue<ISongStream> queue,
            ISongStreamPlayer streamPlayer,
            ISongPlayer songPlayer)
        {
            _queue = queue;
            _streamPlayer = streamPlayer;
            _songPlayer = songPlayer;
        }

        #endregion Constructors

        #region Methods

        public void Start()
        {
            _queue.ItemAdded += QueueOnItemAdded;
            _streamPlayer.NextSong += StreamPlayerOnNextSong;
            _songPlayer.CurrentSongCompleted += SongPlayerOnCurrentSongCompleted;
        }

        private void SongPlayerOnCurrentSongCompleted(object sender, SongEventArgs e)
        {
            if (!_streamPlayer.Next(CancellationToken.None))
            {
                VoteableQueueItem<ISongStream> item;
                if (_queue.TryGetNext(out item))
                {
                    _streamPlayer.CurrentStream = item.Item;
                    _streamPlayer.Next(CancellationToken.None);
                }
            }
        }

        private void StreamPlayerOnNextSong(object sender, SongEventArgs e)
        {
            if (e.Song != null)
            {
                _songPlayer.Start(e.Song);
            }
        }

        private void QueueOnItemAdded(object sender, QueueEventArgs<VoteableQueueItem<ISongStream>> e)
        {
            if (_queue.IsInFront(e.Item))
            {
                _streamPlayer.CurrentStream = e.Item.Item;
                _streamPlayer.Next(CancellationToken.None);
            }
        }

        #endregion Methods
    }
}