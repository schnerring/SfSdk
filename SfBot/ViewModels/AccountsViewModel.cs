using System;
using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using SfBot.Data;
using SfBot.Events;
using SfSdk;

namespace SfBot.ViewModels
{
    [Export(typeof (AccountsViewModel))]
    public class AccountsViewModel : BusyScreen
    {
        private readonly IEventAggregator _events;
        private Account _selectedAccount;

        [ImportingConstructor]
        public AccountsViewModel(IEventAggregator events)
        {
            _events = events;
        }

        [Import]
        public IWindowManager WindowManager { get; private set; }

        [Import]
        public CreateAccountViewModel CreateAccountViewModel { get; private set; }

        [Import]
        public ConfigurationStore ConfigurationStore { get; private set; }
        
        public Account SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                NotifyOfPropertyChange(() => SelectedAccount);
                NotifyOfPropertyChange(() => CanDeleteAccount);
                NotifyOfPropertyChange(() => CanLoginAsync);
                NotifyOfPropertyChange(() => CanLogoutAsync);
                _events.Publish(new AccountChangedEvent(_selectedAccount));
            }
        }

        public bool CanDeleteAccount
        {
            get { return _selectedAccount != null; }
        }

        public bool CanLoginAsync
        {
            get { return _selectedAccount != null && !_selectedAccount.IsLoggedIn; }
        }

        public bool CanLogoutAsync
        {
            get { return _selectedAccount != null && _selectedAccount.IsLoggedIn; }
        }

        public void AddAccount()
        {
            CreateAccountViewModel.Callback = (s, r) =>
                {
                    if (r != true) return;
                    var vm = (CreateAccountViewModel) s;
                    if (!ConfigurationStore.AddAccount(new Account(vm.Username, vm.PasswordHash, vm.SelectedCountry,
                                                                   vm.SelectedServer)))
                        MessageBox.Show("An account with the same username and server exists, already.",
                                        "Adding account failed");
                };
            WindowManager.ShowDialog(CreateAccountViewModel);
        }

        public void DeleteAccount()
        {
            if (_selectedAccount.IsLoggedIn) throw new NotImplementedException();
            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to delete this account?",
                                                            "Submit deletion", MessageBoxButton.YesNo);
            if (dialogResult != MessageBoxResult.Yes) return;
            if (!ConfigurationStore.DeleteAccount(_selectedAccount))
                MessageBox.Show("Account could not be found.", "Account deletion failed");
        }

        public async void LoginAsync()
        {
            IsBusy = true;
            var session = new Session();
            _selectedAccount.Session = session;
            _events.Publish(new LogEvent(session, "Login request started"));
            BusyMessage = "Logging in...";
            var loginSuccessful = await session.LoginAsync(_selectedAccount.Username, _selectedAccount.PasswordHash, _selectedAccount.Server.ServerUri);
            _events.Publish(new LogEvent(session, "Login request finished"));
            if (!loginSuccessful)
            {
                _events.Publish(new LogEvent(session, "Login request failed"));
                _selectedAccount.Session = null;
                MessageBox.Show("Login failed. Please check the server status and your account credentials.",
                                "Login failed");}
            else
            {
                _events.Publish(new LogEvent(session, "Login request succeeded"));
                NotifyOfPropertyChange(() => CanLoginAsync);
                NotifyOfPropertyChange(() => CanLogoutAsync);
                _events.Publish(new LoginStatusChangedEvent(_selectedAccount, true));
            }
            IsBusy = false;
        }

        public async void LogoutAsync()
        {
            IsBusy = true;
            _events.Publish(new LogEvent(_selectedAccount.Session, "Logout request started"));
            BusyMessage = "Logging out...";
            bool isLoggedOut = await _selectedAccount.Session.LogoutAsync();

            if (!isLoggedOut)
            {
                // TODO Retry?
                _events.Publish(new LogEvent(_selectedAccount.Session, "Logout request failed"));
                MessageBox.Show("Logout failed.", "Logout failed");
            }
            else
            {
                _events.Publish(new LogEvent(_selectedAccount.Session, "Logout request succeeded"));
                _selectedAccount.Session = null;
                NotifyOfPropertyChange(() => CanLoginAsync);
                NotifyOfPropertyChange(() => CanLogoutAsync);
                _events.Publish(new LoginStatusChangedEvent(_selectedAccount, false));
            }
            IsBusy = false;
        }
    }
}