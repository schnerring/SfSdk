using Caliburn.Micro;

namespace SfBot
{
    public class BusyScreen : Screen
    {
        private bool _isBusy;
        private string _busyMessage;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        public string BusyMessage
        {
            get { return _busyMessage; }
            set
            {
                _busyMessage = value;
                NotifyOfPropertyChange(() => BusyMessage);
            }
        }
    }
}