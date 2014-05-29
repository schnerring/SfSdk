using System.Threading.Tasks;
using SfBot.Data;
using SfSdk;

namespace SfBot.ViewModels.Details
{
    public abstract class SessionScreenBase : BusyScreen, ISessionScreen, ILoadableScreen
    {
        protected Account Account;

        public virtual void InitAccount(Account account)
        {
            Account = account;
        }

        public virtual async void Load()
        {
            await LoadAsync();
        }

        public abstract Task LoadAsync();
    }
}