using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Caliburn.Micro;
using SfSdk.Contracts;
using SfSdk.Providers;

namespace SfBot.ViewModels
{
    public class CreateAccountViewModel : BusyScreen
    {
        private readonly ObservableCollection<ICountry> _countries = new ObservableCollection<ICountry>();
        private readonly ObservableCollection<IServer> _servers = new ObservableCollection<IServer>();
        public Action<Screen, bool?> Callback;
        private string _passwordHash;
        private ICountry _selectedCountry;
        private IServer _selectedServer;
        private string _username;

        public CreateAccountViewModel()
        {
            base.DisplayName = "Create Accout";
            LoadCountries();
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
            }
        }

        public string PasswordHash
        {
            get { return _passwordHash; }
            private set
            {
                _passwordHash = value;
                NotifyOfPropertyChange(() => PasswordHash);
            }
        }

        public ICountry SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                _selectedCountry = value;
                Servers.Clear();
                if (_selectedCountry != null)
                {
                    foreach (IServer server in _selectedCountry.Servers)
                    {
                        Servers.Add(server);
                    }
                }
                NotifyOfPropertyChange(() => SelectedCountry);
            }
        }

        public IServer SelectedServer
        {
            get { return _selectedServer; }
            set
            {
                _selectedServer = value;
                NotifyOfPropertyChange(() => SelectedServer);
            }
        }

        public ObservableCollection<ICountry> Countries
        {
            get { return _countries; }
        }

        public ObservableCollection<IServer> Servers
        {
            get { return _servers; }
        }

        private async void LoadCountries()
        {
            await LoadCountriesAsync();
        }

        private async Task LoadCountriesAsync()
        {
            IsBusy = true;
            BusyMessage = "Loading Countries...";

            IEnumerable<ICountry> countries = await new CountryProvider().GetCountriesAsync();

            foreach (ICountry country in countries.Where(country => !Countries.Contains(country)).OrderBy(c => c.CountryName))
                Countries.Add(country);

            IsBusy = false;
        }

        public void Ok(SecureString securePassword)
        {
            PasswordHash = securePassword.ConvertToUnsecureString().ToMd5Hash();
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }

        public override void TryClose(bool? dialogResult)
        {
            if (Callback != null)
            {
                Callback(this, dialogResult);
            }
            base.TryClose(dialogResult);
        }
    }
}