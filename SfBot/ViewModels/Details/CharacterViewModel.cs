using System;
using System.Threading.Tasks;
using SfBot.Data;
using SFBot.ViewModels.Details;
using SfSdk.Contracts;

namespace SfBot.ViewModels.Details
{
    public class CharacterViewModel : SessionScreenBase
    {
        public CharacterDetailsViewModel CharacterDetailsViewModel { get; set; }

        public CharacterViewModel(CharacterDetailsViewModel characterDetailsViewModel)
        {
            base.DisplayName = "Character";
            CharacterDetailsViewModel = characterDetailsViewModel;
        }

        public override void InitAccount(Account account)
        {
            base.InitAccount(account);
            CharacterDetailsViewModel.InitAccount(account);
            Func<Account, Task<ICharacter>> myCharacter = async a => await a.Session.MyCharacterAsync();
            CharacterDetailsViewModel.InitCharacterFunc(myCharacter);
        }

        public override async Task LoadAsync()
        {
            return;
        }

        protected override void OnActivate()
        {
 	        base.OnActivate();
            CharacterDetailsViewModel.Load();
        }
    }
}