using System;
using System.Diagnostics;
using SfSdk.Contracts;

namespace SfSdk.Data
{
    /// <summary>
    ///     An inventory item containing the base properties of any inventory item.
    /// </summary>
    [DebuggerDisplay("{Id} ({ContentId}), {Text}")]
    internal class InventoryItem : IInventoryItem
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public string Text { get; set; }
        public string HintText { get; set; }
        public Uri ImageUri { get; set; }
    }
}
