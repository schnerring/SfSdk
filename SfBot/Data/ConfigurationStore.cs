using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Caliburn.Micro;
using SfBot.Events;

namespace SfBot.Data
{
    [Export(typeof (ConfigurationStore))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ConfigurationStore
    {
        private readonly IEventAggregator _events;
        private readonly BinaryFormatter _formatter = new BinaryFormatter();
        private readonly IsolatedStorageFile _store = IsolatedStorageFile.GetUserStoreForAssembly();

        [ImportingConstructor]
        public ConfigurationStore(IEventAggregator events)
        {
            _events = events;
            Accounts = new ObservableCollection<Account>();
            Load();
        }

        public ObservableCollection<Account> Accounts { get; private set; }

        public void Load()
        {
            using (IsolatedStorageFileStream stream = _store.OpenFile("accounts.cfg", FileMode.OpenOrCreate,
                                                                      FileAccess.Read))
            {
                if (stream.Length <= 0) return;
                var accList = (List<Account>) _formatter.Deserialize(stream);
                Accounts =
                    new ObservableCollection<Account>(
                        accList.OrderBy(a => a.Country.Name).ThenBy(a => a.Server.Name).ThenBy(a => a.Username));
            }
        }

        public bool AddAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException("account");
            if (!Accounts.Any(a => a.Username == account.Username && a.Server.Uri == account.Server.Uri))
            {
                Accounts.Add(account);
                _events.Publish(new AccountAddedEvent(account));
                Save();
                return true;
            }
            return false;
        }

        public bool DeleteAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException("account");
            if (Accounts.Any(a => a == account))
            {
                Accounts.Remove(account);
                _events.Publish(new AccountDeletedEvent(account));
                Save();
                return true;
            }
            return false;
        }

        private void Save()
        {
            using (IsolatedStorageFileStream stream = _store.OpenFile("accounts.cfg", FileMode.OpenOrCreate,
                                                                      FileAccess.Write))
            {
                _formatter.Serialize(stream, Accounts.ToList());
            }
        }
    }
}