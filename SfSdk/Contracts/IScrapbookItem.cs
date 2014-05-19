using System;

namespace SfSdk.Contracts
{
    internal interface IScrapbookItem
    {
        string Text { get; set; }

        string HintText { get; set; }

        bool HasItem { get; set; }

        Uri ImageUri { get; set; }
    }
}