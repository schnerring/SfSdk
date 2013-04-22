using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace SFBot.ViewModels
{
    [Export(typeof(LoggedOutViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class LoggedOutViewModel : Screen
    {
    }
}