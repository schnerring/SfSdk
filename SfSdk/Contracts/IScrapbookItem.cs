using System;

namespace SfSdk.Contracts
{
    /// <summary>
    ///     A scrapbook item containing the base properties of any scrapbook item.
    /// </summary>
    public interface IScrapbookItem
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
        ///     Returns true if the scrapbook owner has the item.
        /// </summary>
        bool HasItem { get; }

        /// <summary>
        ///     The item's image <see cref="Uri"/>.
        /// </summary>
        Uri ImageUri { get; }
    }
}