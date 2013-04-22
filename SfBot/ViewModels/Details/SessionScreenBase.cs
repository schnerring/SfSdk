using System.Threading.Tasks;
using SfSdk;

namespace SfBot.ViewModels.Details
{
    public abstract class SessionScreenBase : BusyScreen, ISessionScreen, ILoadableScreen
    {
        protected Session Session;

        public virtual void Init(Session session)
        {
            Session = session;
        }

        public virtual async void Load()
        {
            await LoadAsync();
        }

        public abstract Task LoadAsync();
    }
}