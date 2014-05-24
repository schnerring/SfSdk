using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using SfBot.Events;
using SfBot.ViewModels.Details;
using SfSdk.Contracts;

namespace SFBot.ViewModels.Details
{
    [Export(typeof(ScrapbookViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ScrapbookViewModel : SessionScreenBase
    {
        private readonly IEventAggregator _events;

        [Import]
        public ScrapbookItemViewModel MonsterViewModel { get; set; }
        [Import]
        public ScrapbookItemViewModel ValuableViewModel { get; set; }
        [Import]
        public ScrapbookItemViewModel WarriorViewModel { get; set; }
        [Import]
        public ScrapbookItemViewModel MageViewModel { get; set; }
        [Import]
        public ScrapbookItemViewModel ScoutViewModel { get; set; }

        [ImportingConstructor]
        public ScrapbookViewModel(IEventAggregator events)
        {
            _events = events;
            base.DisplayName = "Scrapbook";
        }

        public override async Task LoadAsync()
        {
            IsBusy = true;
            
            _events.Publish(new LogEvent(Account, "Scrapbook request started"));
            var items = (await Account.Session.ScrapbookAsync()).ToList();
            _events.Publish(new LogEvent(Account, "Scrapbook request finished"));

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
