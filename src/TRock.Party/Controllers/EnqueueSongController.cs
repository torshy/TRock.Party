using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRock.Music;
using System.Linq;

namespace TRock.Party.Controllers
{
    public class EnqueueSongController : ApiController
    {
        #region Fields

        private readonly IVoteableQueue<ISongStream> _queueService;

        #endregion Fields

        #region Constructors

        public EnqueueSongController(IVoteableQueue<ISongStream> queueService)
        {
            _queueService = queueService;
        }

        #endregion Constructors

        #region Methods

        public HttpResponseMessage Song(Song song)
        {
            // Check if the queue contains a song with the same Id. If it does we return with NotModified
            if (_queueService.CurrentQueue.Any(i =>
            {
                if (i.Bag is Song)
                {
                    return i.Bag.Id == song.Id;
                }

                return false;
            }))
            {
                return new HttpResponseMessage(HttpStatusCode.NotModified);
            }

            var stream = new SingleSongStream(song)
            {
                Name = song.Name, Description = song.Artist.Name
            };

            _queueService.Enqueue(stream, setup =>
            {
                setup.Bag = song;
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        #endregion Methods
    }
}
