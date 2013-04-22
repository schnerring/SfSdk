using System.Windows;
using SfBot.ViewModels;

namespace SfBot.Views
{
    public partial class CreateAccountView
    {
        public CreateAccountView()
        {
            InitializeComponent();
        }

        private void Ok(object sender, RoutedEventArgs e)
        {
            using (var secureString = PasswordBox.SecurePassword)
            {
                var vm = (CreateAccountViewModel) DataContext;
                secureString.MakeReadOnly();
                vm.Ok(secureString);
            }
        }
    }
}
