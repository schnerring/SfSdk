using SfBot.Data;

namespace SfBot.Events
{
    public class LoginStatusChangedEvent
    {
        public Account Account { get; set; }
        public bool IsLoggedIn { get; set; }

        public LoginStatusChangedEvent(Account selectedAccount, bool isLoggedIn)
        {
            Account = selectedAccount;
            IsLoggedIn = isLoggedIn;
        }
    }
}