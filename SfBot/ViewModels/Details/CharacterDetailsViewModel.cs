using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using SfBot.Events;
using SfBot.ViewModels.Details;
using SfSdk.Contracts;

namespace SFBot.ViewModels.Details
{
    public class CharacterDetailsViewModel : SessionScreenBase
    {
        private readonly IEventAggregator _events;
        private Func<Task<ICharacter>> _getCharacterAsync;
        private ICharacter _character;

        public CharacterDetailsViewModel(IEventAggregator events)
        {
            _events = events;
        }

        public ICharacter Character
        {
            get { return _character; }
            set
            {
                if (Equals(value, _character)) return;
                _character = value;
                NotifyOfPropertyChange(() => Character);
            }
        }

        public void InitCharacterFunc(Func<Task<ICharacter>> getCharacterAsync)
        {
            if (getCharacterAsync == null) throw new ArgumentNullException("getCharacterAsync");
            _getCharacterAsync = getCharacterAsync;
        }

        public override async Task LoadAsync()
        {
            IsBusy = true;
            _events.PublishOnCurrentThread(new LogEvent(Account, "Character request started"));
            Character = await _getCharacterAsync();
            _events.PublishOnCurrentThread(new LogEvent(Account, "Character request finished"));
            IsBusy = false;
        }
    }
}
