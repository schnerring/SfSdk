using System;
using System.Diagnostics;
using SfSdk.Contracts;

namespace SfSdk.Data
{
    /// <summary>
    ///     A scrapbook item containing the base properties of any scrapbook item.
    /// </summary>
    [DebuggerDisplay("{ItemId} ({ContentId}), {Text}")]
    internal abstract class ScrapbookItemBase : IScrapbookItem
    {
        public int ItemId { get; set; }
        
        public int ContentId { get; set; }

        public string Text { get; set; }

        public string HintText { get; set; }

        public bool HasItem { get; set; }

        public Uri ImageUri { get; set; }
    }
}