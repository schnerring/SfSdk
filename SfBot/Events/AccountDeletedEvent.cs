using SfBot.Data;

namespace SfBot.Events
{
    public class AccountDeletedEvent
    {
        public Account Account { get; set; }

        public AccountDeletedEvent(Account account)
        {
            Account = account;
        }
    }
}