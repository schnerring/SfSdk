using System.Collections.Generic;

namespace SfSdk.Contracts
{
    /// <summary>
    ///     An inventory containing <see cref="IInventoryItem"/>s.
    /// </summary>
    public interface IInventory
    {
        /// <summary>
        ///     A list containing all inventory items.
        /// </summary>
        IList<IInventoryItem> AllItems { get; }

        /// <summary>
        ///     A list containing all backpack items.
        /// </summary>
        IList<IInventoryItem> BackpackItems { get; }

        /// <summary>
        ///     The helmet.
        /// </summary>
        IInventoryItem Helmet { get; }
        
        /// <summary>
        ///     The armor.
        /// </summary>
        IInventoryItem Armor { get; }

        /// <summary>
        ///     The gloves.
        /// </summary>
        IInventoryItem Gloves { get; }

        /// <summary>
        ///     The boots.
        /// </summary>
        IInventoryItem Boots { get; }

        /// <summary>
        ///     The necklace.
        /// </summary>
        IInventoryItem Necklace { get; }

        /// <summary>
        ///     The belt
        /// </summary>
        IInventoryItem Belt { get; }

        /// <summary>
        ///     The ring.
        /// </summary>
        IInventoryItem Ring { get; }

        /// <summary>
        ///     The mojo.
        /// </summary>
        IInventoryItem Mojo { get; }

        /// <summary>
        ///     The weapon.
        /// </summary>
        IInventoryItem Weapon { get; }

        /// <summary>
        ///     The shield.
        /// </summary>
        IInventoryItem Shield { get; }
    }
}