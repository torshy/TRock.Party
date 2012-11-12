using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRock.Music;

namespace TRock.Party.Controllers
{
    public class EnqueueSongsController : ApiController
    {
        #region Fields

        private readonly IVoteableQueue<ISongStream> _queueService;

        #endregion Fields

        #region Constructors

        public EnqueueSongsController(IVoteableQueue<ISongStream> queueService)
        {
            _queueService = queueService;
        }

        #endregion Constructors

        #region Methods

        public HttpResponseMessage Songs(IEnumerable<Song> songs)
        {
            songs = songs.ToArray();

            if (songs.Any())
            {
                _queueService.Enqueue(new MultiSongStream(songs), setup =>
                {
                    setup.Bag = new
                    {
                        setup.Item.Name, 
                        setup.Item.Description
                    };
                });

                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotModified);
        }

        #endregion Methods
    }
}
