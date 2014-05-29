using System.Threading.Tasks;
using Caliburn.Micro;
using SfBot;
using SfBot.Data;
using SfBot.ViewModels.Details;

namespace SFBot.ViewModels.Details
{
    public abstract class SessionConductorBase<T> : BusyConductor<T>, ISessionScreen, ILoadableScreen where T: Screen
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
