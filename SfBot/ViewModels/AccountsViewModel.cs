using System;
using System.Windows;
using Caliburn.Micro;
using SfBot.Data;
using SfBot.Events;
using SfSdk;

namespace SfBot.ViewModels
{
    public class AccountsViewModel : BusyScreen
    {
        private readonly IEventAggregator _events;
        private Account _selectedAccount;

        public AccountsViewModel(IEventAggregator events,
                                 IWindowManager windowManager,
                                 CreateAccountViewModel createAccountViewModel,
                                 ConfigurationStore configurationStore)
        {
            _events = events;
            WindowManager = windowManager;
            CreateAccountViewModel = createAccountViewModel;
            ConfigurationStore = configurationStore;
        }

        public IWindowManager WindowManager { get; private set; }

        public CreateAccountViewModel CreateAccountViewModel { get; private set; }

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
                _events.PublishOnCurrentThread(new AccountChangedEvent(_selectedAccount));
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
            _selectedAccount.Session = SessionCreator.Create();
            _events.PublishOnCurrentThread(new LogEvent(_selectedAccount, "Login request started"));
            BusyMessage = "Logging in...";
            var loginSuccessful = await _selectedAccount.Session.LoginAsync(_selectedAccount.Username, _selectedAccount.PasswordHash, _selectedAccount.Server.ServerUri);
            _events.PublishOnCurrentThread(new LogEvent(_selectedAccount, "Login request finished"));
            if (!loginSuccessful)
            {
                _events.PublishOnCurrentThread(new LogEvent(_selectedAccount, "Login request failed"));
                _selectedAccount.Session = null;
                MessageBox.Show("Login failed. Please check the server status and your account credentials.",
                                "Login failed");}
            else
            {
                _events.PublishOnCurrentThread(new LogEvent(_selectedAccount, "Login request succeeded"));
                NotifyOfPropertyChange(() => CanLoginAsync);
                NotifyOfPropertyChange(() => CanLogoutAsync);
                _events.PublishOnCurrentThread(new LoginStatusChangedEvent(_selectedAccount, true));
            }
            IsBusy = false;
        }

        public async void LogoutAsync()
        {
            IsBusy = true;
            _events.PublishOnCurrentThread(new LogEvent(_selectedAccount, "Logout request started"));
            BusyMessage = "Logging out...";
            bool isLoggedOut = await _selectedAccount.Session.LogoutAsync();

            if (!isLoggedOut)
            {
                // TODO Retry?
                _events.PublishOnCurrentThread(new LogEvent(_selectedAccount, "Logout request failed"));
                MessageBox.Show("Logout failed.", "Logout failed");
            }
            else
            {
                _events.PublishOnCurrentThread(new LogEvent(_selectedAccount, "Logout request succeeded"));
                _selectedAccount.Session = null;
                NotifyOfPropertyChange(() => CanLoginAsync);
                NotifyOfPropertyChange(() => CanLogoutAsync);
                _events.PublishOnCurrentThread(new LoginStatusChangedEvent(_selectedAccount, false));
            }
            IsBusy = false;
        }
    }
}