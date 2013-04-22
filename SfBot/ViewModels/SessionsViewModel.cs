using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using SFBot.ViewModels;
using SfBot.Data;
using SfBot.Events;
using SfBot.Views;

namespace SfBot.ViewModels
{
    [Export(typeof (SessionsViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SessionsViewModel : Conductor<Screen>,
                                     IHandle<AccountChangedEvent>,
                                     IHandle<AccountDeletedEvent>,
                                     IHandle<LoginStatusChangedEvent>
    {
        private static readonly Dictionary<Account, DetailsViewModel> SessionsDict =
            new Dictionary<Account, DetailsViewModel>();

        private readonly Func<Account, DetailsViewModel> _detailsVmFactory;
        private Account _selectedAccount;

        [ImportingConstructor]
        public SessionsViewModel(IEventAggregator events, Func<Account, DetailsViewModel> detailsVmFactory)
        {
            _detailsVmFactory = detailsVmFactory;
            events.Subscribe(this);
        }

        public Account SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                NotifyOfPropertyChange(() => SelectedAccount);
                NotifyOfPropertyChange(() => IsLoggedIn);
                NotifyOfPropertyChange(() => IsCovered);
            }
        }

        public bool IsLoggedIn
        {
            get { return _selectedAccount != null && _selectedAccount.IsLoggedIn; }
        }

        public bool IsCovered
        {
            get { return !IsLoggedIn; }
        }

        public void Handle(AccountChangedEvent message)
        {
            SelectedAccount = message.Account;
            if (_selectedAccount == null)
            {
                ActivateItem(null);
                return;
            }
            if (!SessionsDict.ContainsKey(_selectedAccount))
            {
                DetailsViewModel vm = _detailsVmFactory(_selectedAccount);
                SessionsDict.Add(_selectedAccount, vm);
            }
            if (_selectedAccount.IsLoggedIn) ActivateItem(SessionsDict[_selectedAccount]);
            else ActivateItem(IoC.Get<LoggedOutViewModel>());
        }

        public void Handle(AccountDeletedEvent message)
        {
            Account acc = message.Account;
            if (acc == null) return;
            if (SessionsDict.ContainsKey(acc))
                SessionsDict.Remove(acc);
        }

        public void Handle(LoginStatusChangedEvent message)
        {
            if (_selectedAccount.IsLoggedIn) ActivateItem(SessionsDict[_selectedAccount]);
            else ActivateItem(IoC.Get<LoggedOutViewModel>());
            NotifyOfPropertyChange(() => SelectedAccount);
            NotifyOfPropertyChange(() => IsLoggedIn);
            NotifyOfPropertyChange(() => IsCovered);
        }

        public override void ActivateItem(Screen item)
        {
            base.ActivateItem(item);
            var v = GetView();
            ((SessionsView) v).AnimatedContent.Reload();
        }
    }
}