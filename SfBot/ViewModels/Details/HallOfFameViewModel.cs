using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using SfSdk.Contracts;

namespace SfBot.ViewModels.Details
{
    [Export(typeof(HallOfFameViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HallOfFameViewModel : SessionScreenBase
    {
        private readonly BindableCollection<ICharacter> _characters = new BindableCollection<ICharacter>();

        public BindableCollection<ICharacter> Characters
        {
            get { return _characters; }
        }

        public HallOfFameViewModel()
        {
            base.DisplayName = "Hall Of Fame";
        }

        public override async Task LoadAsync()
        {
            IsBusy = true;
            _characters.AddRange(await Session.HallOfFameAsync());
            IsBusy = false;
        }

        protected override void OnActivate()
        {
            Load();
            base.OnActivate();
        }
    }
}