using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using SfBot.Data;
using SfBot.Events;
using SfBot.ViewModels.Details;
using SfSdk.Contracts;
using SfSdk.Framework;

namespace SFBot.ViewModels.Details
{
    public class HallOfFameCrawlerViewModel : SessionScreenBase
    {
        private readonly HallOfFameSearchPredicate _searchPredicate = new HallOfFameSearchPredicate();
        private HallOfFameCrawler _crawler;
        private readonly IEventAggregator _events;
        private bool _canSearchAsync = true;
        private string _excludedUsernames = string.Empty;
        private string _excludedGuilds = string.Empty;
        private int _maxLevel = 300;
        private int _minLevel = 1;
        private int _maxHonor = 100000;
        private int _minHonor = 1;
        private int _maxRank = 67;
        private int _minRank = 1;
        private readonly BindableCollection<ICharacter> _searchResults = new BindableCollection<ICharacter>();

        public HallOfFameCrawlerViewModel(IEventAggregator events)
        {
            base.DisplayName = "Hall Of Fame Crawler";
            _events = events;
        }

        public override void InitAccount(Account account)
        {
            base.InitAccount(account);
            
            _crawler = new HallOfFameCrawler(account.Session);
        }

        public override async Task LoadAsync()
        {
            // fill stored search results
            return;
        }

        public async void SearchAsync()
        {
            IsBusy = true;
            CanSearchAsync = false;

            _searchPredicate.MinRank = MinRank;
            _searchPredicate.MaxRank = MaxRank;

            _searchPredicate.MinHonor = MinHonor;
            _searchPredicate.MaxHonor = MaxHonor;

            _searchPredicate.MinLevel = MinLevel;
            _searchPredicate.MaxLevel = MaxLevel;

            _searchPredicate.ExcludedUsernames = ExcludedUsernames.Split(';').ToList();
            _searchPredicate.ExcludedGuilds = ExcludedGuilds.Split(';').ToList();
            
            ChunkCompletedEventHandler chunkCompleted = (s, e) =>
            {
                var percentage = e.FinishedChunks*100/e.TotalChunks;;
                var message = string.Format("processing chunks... {0} / {1} completed.", e.FinishedChunks, e.TotalChunks);
                BusyMessage = percentage + "%";
                _events.PublishOnCurrentThread(new LogEvent(Account, message));
            };

            _crawler.ChunkCompleted += chunkCompleted;
            var searchResults = await _crawler.SearchAsync(_searchPredicate);
            _crawler.ChunkCompleted -= chunkCompleted;

            _searchResults.Clear();
            foreach (var searchResult in searchResults)
            {
                _searchResults.Add(searchResult);
            }

            CanSearchAsync = true;
            IsBusy = false;
        }

        public HallOfFameSearchPredicate SearchPredicate
        {
            get { return _searchPredicate; }
        }

        public BindableCollection<ICharacter> SearchResults
        {
            get { return _searchResults; }
        }

        public string ExcludedUsernames
        {
            get { return _excludedUsernames; }
            set
            {
                if (value == _excludedUsernames) return;
                _excludedUsernames = value;
                NotifyOfPropertyChange(() => ExcludedUsernames);
            }
        }

        public string ExcludedGuilds
        {
            get { return _excludedGuilds; }
            set
            {
                if (value == _excludedGuilds) return;
                _excludedGuilds = value;
                NotifyOfPropertyChange(() => ExcludedGuilds);
            }
        }

        public int MaxLevel
        {
            get { return _maxLevel; }
            set
            {
                if (value == _maxLevel) return;
                _maxLevel = value;
                NotifyOfPropertyChange(() => MaxLevel);
            }
        }

        public int MinLevel
        {
            get { return _minLevel; }
            set
            {
                if (value == _minLevel) return;
                if (value > _maxLevel)
                {
                    _minLevel = _maxLevel;
                }
                _minLevel = value;
                NotifyOfPropertyChange(() => MinLevel);
            }
        }

        public int MaxHonor
        {
            get { return _maxHonor; }
            set
            {
                if (value == _maxHonor) return;
                _maxHonor = value;
                NotifyOfPropertyChange(() => MaxHonor);
            }
        }

        public int MinHonor
        {
            get { return _minHonor; }
            set
            {
                if (value == _minHonor) return;
                _minHonor = value;
                NotifyOfPropertyChange(() => MinHonor);
            }
        }

        public int MaxRank
        {
            get { return _maxRank; }
            set
            {
                if (value == _maxRank) return;
                _maxRank = value;
                NotifyOfPropertyChange(() => MaxRank);
            }
        }

        public int MinRank
        {
            get { return _minRank; }
            set
            {
                if (value == _minRank) return;
                _minRank = value;
                NotifyOfPropertyChange(() => MinRank);
            }
        }

        public bool CanSearchAsync
        {
            get { return _canSearchAsync; }
            set
            {
                if (value == _canSearchAsync) return;
                _canSearchAsync = value;
                NotifyOfPropertyChange(() => CanSearchAsync);
            }
        }
    }
}