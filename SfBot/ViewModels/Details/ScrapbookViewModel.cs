using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Drawing;
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
        private readonly BindableCollection<IScrapbookItem> _scrapbookItems = new BindableCollection<IScrapbookItem>();
        private readonly BindableCollection<IMonsterItem> _monsterItems = new BindableCollection<IMonsterItem>();
        private readonly BindableCollection<IValuableItem> _valuableItems = new BindableCollection<IValuableItem>();
        private readonly BindableCollection<IWarriorItem> _warriorItems = new BindableCollection<IWarriorItem>();
        private readonly BindableCollection<IMageItem> _mageItems = new BindableCollection<IMageItem>();
        private readonly BindableCollection<IScoutItem> _scoutItems = new BindableCollection<IScoutItem>();

        public BindableCollection<IMonsterItem> MonsterItems
        {
            get { return _monsterItems; }
        }

        public BindableCollection<IValuableItem> ValuableItems
        {
            get { return _valuableItems; }
        }

        public BindableCollection<IWarriorItem> WarriorItems
        {
            get { return _warriorItems; }
        }

        public BindableCollection<IMageItem> MageItems
        {
            get { return _mageItems; }
        }

        public BindableCollection<IScoutItem> ScoutItems
        {
            get { return _scoutItems; }
        }

        [ImportingConstructor]
        public ScrapbookViewModel(IEventAggregator events)
        {
            _events = events;
            base.DisplayName = "Scrapbook";
        }

        public override async Task LoadAsync()
        {
            IsBusy = true;
            _events.Publish(new LogEvent(Account.Session, "Scrapbook request started"));

            var items = (await Account.Session.ScrapbookAsync()).ToList();

            foreach (var monsterItem in items.OfType<IMonsterItem>()) _monsterItems.Add(monsterItem);
            foreach (var valuableItem in items.OfType<IValuableItem>()) _valuableItems.Add(valuableItem);
            foreach (var warriorItem in items.OfType<IWarriorItem>()) _warriorItems.Add(warriorItem);
            foreach (var mageItem in items.OfType<IMageItem>()) _mageItems.Add(mageItem);
            foreach (var scoutItem in items.OfType<IScoutItem>()) _scoutItems.Add(scoutItem);

            _events.Publish(new LogEvent(Account.Session, "Scrapbook request finished"));
            IsBusy = false;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Load();
        }
    }
}
