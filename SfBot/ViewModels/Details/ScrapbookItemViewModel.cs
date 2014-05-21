using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using SfSdk.Contracts;

namespace SFBot.ViewModels.Details
{
    [Export(typeof(ScrapbookItemViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ScrapbookItemViewModel : Screen
    {
        public BindableCollection<IScrapbookItem> Items { get; private set; }

        public ScrapbookItemViewModel()
        {
            Items = new BindableCollection<IScrapbookItem>();
        }

        public void Init(IEnumerable<IScrapbookItem> items)
        {
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public async override Task LoadAsync()
        {
        }
    }
}