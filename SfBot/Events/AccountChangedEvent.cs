using SfBot.Data;

namespace SfBot.Events
{
    public class AccountChangedEvent
    {
        public Account Account { get; private set; }

        public AccountChangedEvent(Account account)
        {
            Account = account;
        }
    }
}