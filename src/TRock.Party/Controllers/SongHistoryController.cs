using System.Collections.Generic;
using System.Web.Http;
using TRock.Music;
using System.Linq;

namespace TRock.Party.Controllers
{
    public class SongHistoryController : ApiController
    {
        #region Fields

        private readonly ISongHistoryService _songHistoryService;

        #endregion Fields

        #region Constructors

        public SongHistoryController(ISongHistoryService songHistoryService)
        {
            _songHistoryService = songHistoryService;
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<Song> GetHistory()
        {
            return _songHistoryService.SongHistory.ToArray();
        }

        #endregion Methods
    }
}