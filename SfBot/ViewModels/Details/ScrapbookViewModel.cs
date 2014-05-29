using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using SfBot.Events;
using SfBot.ViewModels.Details;
using SfSdk.Contracts;

namespace SFBot.ViewModels.Details
{
    public class ScrapbookViewModel : SessionScreenBase
    {
        private readonly IEventAggregator _events;

        public ScrapbookItemViewModel MonsterViewModel { get; set; }

        public ScrapbookItemViewModel ValuableViewModel { get; set; }

        public ScrapbookItemViewModel WarriorViewModel { get; set; }

        public ScrapbookItemViewModel MageViewModel { get; set; }

        public ScrapbookItemViewModel ScoutViewModel { get; set; }

        public ScrapbookViewModel(IEventAggregator events,
                                  ScrapbookItemViewModel monsterViewModel,
                                  ScrapbookItemViewModel valuableViewModel,
                                  ScrapbookItemViewModel warriorViewModel,
                                  ScrapbookItemViewModel mageViewModel,
                                  ScrapbookItemViewModel scoutViewModel)
        {
            base.DisplayName = "Scrapbook";
            _events = events;
            MonsterViewModel = monsterViewModel;
            ValuableViewModel = valuableViewModel;
            WarriorViewModel = warriorViewModel;
            MageViewModel = mageViewModel;
            ScoutViewModel = scoutViewModel;
        }

        public override async Task LoadAsync()
        {
            IsBusy = true;
            
            _events.PublishOnCurrentThread(new LogEvent(Account, "Scrapbook request started"));
            var items = (await Account.Session.ScrapbookAsync()).ToList();
            _events.PublishOnCurrentThread(new LogEvent(Account, "Scrapbook request finished"));

            MonsterViewModel.DisplayName = "Monster Items";
            MonsterViewModel.Init(items.OfType<IMonsterItem>());

            ValuableViewModel.DisplayName = "Valuable Items";
            ValuableViewModel.Init(items.OfType<IValuableItem>());

            WarriorViewModel.DisplayName = "Warrior Items";
            WarriorViewModel.Init(items.OfType<IWarriorItem>());

            MageViewModel.DisplayName = "Mage Items";
            MageViewModel.Init(items.OfType<IMageItem>());

            ScoutViewModel.DisplayName = "Scout Items";
            ScoutViewModel.Init(items.OfType<IScoutItem>());

            IsBusy = false;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Load();
        }
    }
}
