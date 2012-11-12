using System.Linq;
using System.Threading;

using Autofac;

using TRock.Music;
using TRock.Music.EchoNest;

namespace TRock.Party
{
    public class AutoPlaylistService : IStartable
    {
        #region Fields

        private readonly IVoteableQueue<ISongStream> _queue;
        private readonly ISongProvider _songProvider;
        private readonly ISongStreamPlayer _streamPlayer;

        #endregion Fields

        #region Constructors

        public AutoPlaylistService(
            IVoteableQueue<ISongStream> queue,
            ISongProvider songProvider,
            ISongStreamPlayer streamPlayer)
        {
            _queue = queue;
            _songProvider = songProvider;
            _streamPlayer = streamPlayer;
        }

        #endregion Constructors

        #region Methods

        public void Start()
        {
            _queue.ItemAdded += (sender, args) =>
            {
                VoteableQueueItem<ISongStream> current;
                if (_queue.CurrentQueue.Count() > 1 && _queue.TryPeek(out current))
                {
                    // If the current playing stream is autoplaylist and a user adds a song, we play that added song straight away
                    if (current.Item is SimilarArtistsStream)
                    {
                        if (_queue.TryGetNext(out current))
                        {
                            _streamPlayer.CurrentStream = current.Item;
                            _streamPlayer.Next(CancellationToken.None);
                        }
                    }
                }
            };

            _streamPlayer.CurrentStreamCompleted += (sender, args) =>
            {

            };

            _queue.ItemRemoved += (sender, args) =>
            {
                if (!_queue.CurrentQueue.Any())
                {
                    if (args.Item.Bag is Song)
                    {
                        var song = args.Item.Bag as Song;

                        _queue.Enqueue(new SimilarArtistsStream(_songProvider, song.Artist.Name, "RJOXXESVUVZ07WY1T"), x =>
                        {
                            x.Bag = new
                            {
                                x.Item.Name,
                                x.Item.Description
                            };
                        });
                    }
                }
            };
        }

        #endregion Methods
    }
}