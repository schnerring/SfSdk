using System;

namespace SfSdk.Contracts
{
    /// <summary>
    ///     A inventory item containing the base properties of any inventory item.
    /// </summary>
    public interface IInventoryItem : IItem
    {
        /// <summary>
        ///     The item's text.
        /// </summary>
        string Text { get; }

        /// <summary>
        ///     The item's extended text.
        /// </summary>
        string HintText { get; }

        /// <summary>
        ///     The item's image <see cref="Uri"/>.
        /// </summary>
        Uri ImageUri { get; }
    }
}