using SfBot.Data;

namespace SfBot.Events
{
    public class AccountAddedEvent
    {
        public Account Account { get; set; }

        public AccountAddedEvent(Account account)
        {
            Account = account;
        }
    }
}