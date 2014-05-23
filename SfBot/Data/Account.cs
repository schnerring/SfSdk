using System;
using System.Runtime.Serialization;
using Caliburn.Micro;
using SfSdk;
using SfSdk.Contracts;

namespace SfBot.Data
{
    [Serializable]
    public class Account : PropertyChangedBase, ISerializable
    {
        private ISession _session;

        public Account(string username, string passwordHash, ICountry country, IServer server)
        {
            Username = username;
            PasswordHash = passwordHash;
            Country = country;
            Server = server;
        }

        public bool IsLoggedIn
        {
            get { return _session != null; }
        }

        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public ICountry Country { get; private set; }
        public IServer Server { get; private set; }

        public ISession Session
        {
            get { return _session; }
            set
            {
                _session = value;
                NotifyOfPropertyChange(() => IsLoggedIn);
            }
        }

        #region Serialization

        // TODO Implement serialization properly

        protected Account(SerializationInfo info, StreamingContext context)
        {
            Username = info.GetString("Username");
            PasswordHash = (string) info.GetValue("PasswordHash", typeof (string));
            Country = (ICountry) info.GetValue("Country", typeof (ICountry));
            Server = (IServer) info.GetValue("Server", typeof (IServer));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("PasswordHash", PasswordHash);
            info.AddValue("Country", Country);
            info.AddValue("Server", Server);
        }

        #endregion
    }
}