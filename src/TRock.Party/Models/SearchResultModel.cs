using System.Collections.Generic;
using System.Linq;

using TRock.Music;

namespace TRock.Party.Models
{
    public class SearchResultModel
    {
        #region Properties

        public bool HasResults
        {
            get { return Songs != null && Songs.Any(); }
        }

        public IEnumerable<Song> Songs
        {
            get;
            set;
        }

        public IEnumerable<ArtistAlbum> Artists
        {
            get;
            set;
        }

        public IEnumerable<ArtistAlbum> Albums
        {
            get;
            set;
        }

        public string Query
        {
            get;
            set;
        }

        #endregion Properties
    }
}