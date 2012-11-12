using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using TRock.Music;

namespace TRock.Party.Controllers
{
    public class QueueController : ApiController
    {
        #region Fields

        private readonly IVoteableQueue<ISongStream> _queue;
        private readonly ISongStreamPlayer _streamPlayer;
        private readonly ISongHistoryService _songHistoryService;

        #endregion Fields

        #region Constructors

        public QueueController(
            IVoteableQueue<ISongStream> queue,
            ISongStreamPlayer streamPlayer)
        {
            _queue = queue;
            _streamPlayer = streamPlayer;
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<dynamic> GetQueue()
        {
            return ConvertToWebType(_queue.CurrentQueue);
        }

        public HttpResponseMessage MoveToNext()
        {
            if (_streamPlayer.Next(CancellationToken.None))
            {
                return new HttpResponseMessage();
            }

            return new HttpResponseMessage(HttpStatusCode.NotModified);
        }

        public static IEnumerable<dynamic> ConvertToWebType(IEnumerable<VoteableQueueItem<ISongStream>> queue)
        {
            return queue.Select(i =>
            {
                string itemType;

                if (i.Bag is Song)
                {
                    itemType = "song";
                }
                else
                {
                    itemType = "songs";
                }

                return new
                {
                    i.Id,
                    i.Upvotes,
                    i.Downvotes,
                    Item = i.Bag,
                    ItemType = itemType
                };
            });
        }

        #endregion Methods
    }
}