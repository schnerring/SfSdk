using System.Collections.Generic;
using Caliburn.Micro;
using SfSdk.Contracts;

namespace SFBot.ViewModels.Details
{
    public class ScrapbookItemViewModel : Screen
    {
        public BindableCollection<IScrapbookItem> Items { get; private set; }

        public ScrapbookItemViewModel()
        {
            Items = new BindableCollection<IScrapbookItem>();
        }

        public void FillItems(IEnumerable<IScrapbookItem> items)
        {
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}