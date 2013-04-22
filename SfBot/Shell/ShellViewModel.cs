using System.ComponentModel.Composition;
using Caliburn.Micro;
using SfBot.ViewModels;

namespace SfBot.Shell
{
    [Export(typeof (IShell))]
    public class ShellViewModel : Conductor<Screen>, IShell
    {
        [Import]
        public AccountsViewModel AccountsViewModel { get; set; }
        [Import]
        public SessionsViewModel SessionsViewModel { get; set; }
        [Import]
        public FooterViewModel FooterViewModel { get; set; }

        public ShellViewModel()
        {
            base.DisplayName = "SF Bot";
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            ActivateItem(SessionsViewModel);
        }
    }
}