using System.Threading.Tasks;

namespace SfBot.ViewModels.Details
{
    public interface ILoadableScreen
    {
        void Load();
        Task LoadAsync();
    }
}