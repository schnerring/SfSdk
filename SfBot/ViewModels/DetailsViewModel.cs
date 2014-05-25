using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using SfBot.Data;
using SfBot.Events;
using SfBot.ViewModels.Details;
using SFBot.ViewModels.Details;

namespace SFBot.ViewModels
{
    [Export(typeof (DetailsViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DetailsViewModel : Conductor<Screen>, IHandle<LoginStatusChangedEvent>
    {
        private readonly BindableCollection<ISessionScreen> _items;

        private bool _isLoggedIn;
        private Account _account;

        [ImportingConstructor]
        public DetailsViewModel(IEventAggregator events,
                                CharacterViewModel characterViewModel,
                                HallOfFameCrawlerViewModel hallOfFameCrawlerViewModel,
                                ScrapbookViewModel scrapbookViewModel)
        {
            _items = new BindableCollection<ISessionScreen>
                {
                    characterViewModel,
                    hallOfFameCrawlerViewModel,
                    scrapbookViewModel
                };
            events.Subscribe(this);
        }

        public BindableCollection<ISessionScreen> Items
        {
            get { return _items; }
        }

        public ISessionScreen SelectedItem
        {
            get { return ActiveItem as ISessionScreen; }
            set
            {
                ActivateItem(value as Screen);
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                _isLoggedIn = value;
                NotifyOfPropertyChange(() => IsLoggedIn);
                NotifyOfPropertyChange(() => IsCovered);
            }
        }

        public bool IsCovered
        {
            get { return !IsLoggedIn; }
        }

        public void Handle(LoginStatusChangedEvent message)
        {
            if (_account != message.Account) return;
            IsLoggedIn = message.IsLoggedIn;
            if (!message.IsLoggedIn) return;
            foreach (ISessionScreen sessionScreen in _items)
                sessionScreen.Init(_account);
            SelectedItem = _items.FirstOrDefault();
        }

        public void Init(Account account)
        {
            _account = account;
        }
    }
}