using System.Collections.Concurrent;
using System.Collections.Generic;

using Autofac;

using TRock.Music;

namespace TRock.Party
{
    public class SongHistoryService : IStartable, ISongHistoryService
    {
        #region Fields

        private readonly ConcurrentStack<Song> _songHistory;
        private readonly ISongPlayer _songPlayer;

        private int _maximumLength;

        #endregion Fields

        #region Constructors

        public SongHistoryService(ISongPlayer songPlayer)
        {
            _songPlayer = songPlayer;
            _songHistory = new ConcurrentStack<Song>();
            _maximumLength = 200;
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<Song> SongHistory
        {
            get { return _songHistory; }
        }

        public int MaximumLength
        {
            get
            {
                return _maximumLength;
            }
            set
            {
                _maximumLength = value;
                TrimToMaximumSize();
            }
        }

        #endregion Properties

        #region Methods

        public void Start()
        {
            _songPlayer.CurrentSongChanged += (sender, args) =>
            {
                if (args.NewValue != null)
                {
                    _songHistory.Push(args.NewValue);

                    TrimToMaximumSize();
                }
            };
        }

        private void TrimToMaximumSize()
        {
            while (_songHistory.Count > _maximumLength)
            {
                Song _;
                _songHistory.TryPop(out _);
            }
        }

        #endregion Methods
    }
}