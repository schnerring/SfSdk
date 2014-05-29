using Caliburn.Micro;
using SfBot.ViewModels;

namespace SfBot.Shell
{
    public class ShellViewModel : Conductor<Screen>, IShell
    {
        
        public AccountsViewModel AccountsViewModel { get; set; }
        
        public SessionsViewModel SessionsViewModel { get; set; }
        
        public FooterViewModel FooterViewModel { get; set; }

        public ShellViewModel(AccountsViewModel accountsViewModel,
                              SessionsViewModel sessionViewModel,
                              FooterViewModel footerViewModel)
        {
            base.DisplayName = "SF Bot";
            AccountsViewModel = accountsViewModel;
            SessionsViewModel = sessionViewModel;
            FooterViewModel = footerViewModel;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            ActivateItem(SessionsViewModel);
        }
    }
}