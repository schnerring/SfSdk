using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace SfBot.ViewModels.Details
{
    [Export(typeof(HallOfFameViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HallOfFameViewModel : SessionScreenBase
    {
        public HallOfFameViewModel()
        {
            base.DisplayName = "Hall Of Fame";
        }

        public override async Task LoadAsync()
        {
            IsBusy = true;
//            var c = await Session.RequestCharacterAsync("brigada00");
            IsBusy = false;
        }
    }
}