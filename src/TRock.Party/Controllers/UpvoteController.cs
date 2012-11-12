using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRock.Music;

namespace TRock.Party.Controllers
{
    public class UpvoteController : ApiController
    {       
        #region Fields

        private readonly IVoteableQueue<ISongStream> _queueService;

        #endregion Fields

        #region Constructors

        public UpvoteController(IVoteableQueue<ISongStream> queueService)
        {
            _queueService = queueService;
        }

        #endregion Constructors

        #region Methods

        public HttpResponseMessage Post(long id)
        {
            if (_queueService.Upvote(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        #endregion Methods
    }
}
