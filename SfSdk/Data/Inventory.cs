using System.Collections.Generic;
using System.Linq;
using SfSdk.Contracts;

namespace SfSdk.Data
{
    /// <summary>
    /// An inventory containing <see cref="IInventoryItem"/>s.
    /// </summary>
    public class Inventory : IInventory
    {
        private List<IInventoryItem> _inventoryItems; 

        /// <summary>
        ///     Creates a new instance of <see cref="Inventory"/>.
        /// </summary>
        public Inventory()
        {
            AllItems = new List<IInventoryItem>(15);
        }

        public IList<IInventoryItem> AllItems { get; private set; }

        public IList<IInventoryItem> BackpackItems
        {
            get
            {
                return _inventoryItems ??
                       (_inventoryItems = new List<IInventoryItem>(AllItems.Where(i => AllItems.IndexOf(i) > 9)));
            }
        }

        public IInventoryItem Helmet
        {
            get { return AllItems[0]; }
        }

        public IInventoryItem Armor
        {
            get { return AllItems[1]; }
        }

        public IInventoryItem Gloves
        {
            get { return AllItems[2]; }
        }

        public IInventoryItem Boots
        {
            get { return AllItems[3]; }
        }

        public IInventoryItem Necklace
        {
            get { return AllItems[4]; }
        }

        public IInventoryItem Belt
        {
            get { return AllItems[5]; }
        }

        public IInventoryItem Ring
        {
            get { return AllItems[6]; }
        }

        public IInventoryItem Mojo
        {
            get { return AllItems[7]; }
        }

        public IInventoryItem Weapon 
        {
            get { return AllItems[8]; }
        }

        public IInventoryItem Shield
        {
            get { return AllItems[9]; }
        }
    }
}