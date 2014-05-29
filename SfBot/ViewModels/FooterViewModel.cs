using System;
using System.Diagnostics;
using System.Windows.Threading;
using Caliburn.Micro;
using SfBot.Data;
using SfBot.Events;

namespace SfBot.ViewModels
{
    public class FooterViewModel : Screen, IHandle<AccountChangedEvent>, IHandle<LogEvent>
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};
        private int _counter;

        private string _message;
        private Account _selectedAccount;

        public FooterViewModel(IEventAggregator events)
        {
            events.Subscribe(this);
            _timer.Tick += (sender, args) =>
                {
                    if (_counter > 0) Message = string.Empty;
                    else _counter++;
                };
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        public void Handle(AccountChangedEvent message)
        {
            _selectedAccount = message.Account;
        }

        public void Handle(LogEvent message)
        {
            if (message.Account != _selectedAccount) return;
            _counter = 0;
            Message = message.Message;
            _timer.Start();
        }

        public void GotoGithub()
        {
            Process.Start("https://github.com/ebeeb");
        }
    }
}