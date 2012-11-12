using System.Diagnostics;
using System.Threading;

using Autofac;

using SignalR;

using TRock.Music;
using TRock.Music.EchoNest;
using TRock.Party.Controllers;
using TRock.Party.Hubs;

namespace TRock.Party
{
    public class QueueEventPublisher : IStartable
    {
        #region Fields

        private readonly IVoteableQueue<ISongStream> _queue;
        private readonly ISongPlayer _songPlayer;
        private readonly ISongStreamPlayer _streamPlayer;

        #endregion Fields

        #region Constructors

        public QueueEventPublisher(
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
            var hub = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

            _queue.ItemAdded += (sender, args) =>
            {
                Trace.WriteLine("[ItemAddedEvent]");

                hub.Clients.queueChanged(QueueController.ConvertToWebType(_queue.CurrentQueue));
                hub.Clients.queueItemAdded(args.Item);
            };

            _queue.ItemRemoved += (sender, args) =>
            {
                Trace.WriteLine("[ItemRemovedEvent]");

                hub.Clients.queueChanged(QueueController.ConvertToWebType(_queue.CurrentQueue));
                hub.Clients.queueItemRemoved(args.Item);
            };

            _queue.ItemMoved += (sender, args) =>
            {
                Trace.WriteLine("[ItemMovedEvent]");

                hub.Clients.queueChanged(QueueController.ConvertToWebType(_queue.CurrentQueue));
                hub.Clients.queueItemMoved(args.Item);
            };

            _queue.ItemUpvoted += (sender, args) =>
            {
                Trace.WriteLine("[ItemUpvoted]");
            };

            _queue.ItemDownvoted += (sender, args) =>
            {
                Trace.WriteLine("[ItemDownvoted]");

                if (_queue.IsInFront(args.Item) && args.Item.Score < 0.5)
                {
                    if (args.Item.Item is SimilarArtistsStream)
                    {
                        VoteableQueueItem<ISongStream> item;
                        if (_queue.TryGetNext(out item))
                        {
                            _streamPlayer.CurrentStream = item.Item;
                            _streamPlayer.Next(CancellationToken.None);
                        }
                        else
                        {
                            _songPlayer.Stop();
                        }
                    }
                    else
                    {
                        if (!_streamPlayer.Next(CancellationToken.None))
                        {
                            VoteableQueueItem<ISongStream> item;
                            if (_queue.TryGetNext(out item))
                            {
                                _streamPlayer.CurrentStream = item.Item;
                                _streamPlayer.Next(CancellationToken.None);
                            }
                            else
                            {
                                _songPlayer.Stop();
                            }
                        }
                    }
                }
            };
        }

        #endregion Methods
    }
}