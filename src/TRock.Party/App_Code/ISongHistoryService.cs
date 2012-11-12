using System.Collections.Generic;

using TRock.Music;

namespace TRock.Party
{
    public interface ISongHistoryService
    {
        #region Properties

        IEnumerable<Song> SongHistory
        {
            get;
        }

        int MaximumLength
        {
            get;
            set;
        }

        #endregion Properties
    }
}