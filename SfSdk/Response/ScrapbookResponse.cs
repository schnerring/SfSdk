using System;
using System.Collections.Generic;
using System.Linq;
using SfSdk.Constants;
using SfSdk.Contracts;
using SfSdk.Data;
using SfSdk.Providers;

namespace SfSdk.Response
{
    /// <summary>
    ///     The reponse type returned on <see cref="SF.RespAlbum" />.<br />
    ///     Triggered by action <see cref="SF.ActAlbum" />.
    /// </summary>
    internal interface IScrapbookResponse : IResponse
    {
        List<IScrapbookItem> Items { get; } 
    }

    /// <summary>
    ///     The response type returned on <see cref="SF.RespAlbum" />.<br />
    ///     Triggered by action <see cref="SF.ActAlbum" />.
    /// </summary>
    internal class ScrapbookResponse : ResponseBase, IScrapbookResponse
    {
        public List<IScrapbookItem> Items { get; private set; }

        /// <summary>
        ///     Creates a new scrapbook response.
        /// </summary>
        /// <param name="args">The response arguments.</param>
        /// <param name="serverUri">The server's <see cref="Uri"/>.</param>
        /// <exception cref="ArgumentException">When the arguments have not a minimum length of 1.</exception>
        /// <exception cref="ArgumentNullException">When serverUri is null.</exception>
        public ScrapbookResponse(string[] args, Uri serverUri) : base(args)
        {
            if (Args.Length < 1) throw new ArgumentException("The arguments must have a minimum length of 1.", "args");
            if (serverUri == null) throw new ArgumentNullException("serverUri");
            
            Items = new List<IScrapbookItem>();
            var itemProvider = new ScrapbookItemProvider(serverUri);

            var byteArray = Convert.FromBase64String(Args.First());
            var scrapbookContent = new List<int>();
            foreach (var b in byteArray)
            {
                scrapbookContent.Add((b & 128)/128);
                scrapbookContent.Add((b & 64)/64);
                scrapbookContent.Add((b & 32)/32);
                scrapbookContent.Add((b & 16)/16);
                scrapbookContent.Add((b & 8)/8);
                scrapbookContent.Add((b & 4)/4);
                scrapbookContent.Add((b & 2)/2);
                scrapbookContent.Add(b & 1);
            }

            Items.AddRange(itemProvider.CreateMonsterItems(scrapbookContent));
            Items.AddRange(itemProvider.CreateValuableItems(scrapbookContent));
            Items.AddRange(itemProvider.CreateWarriorItems(scrapbookContent));
            Items.AddRange(itemProvider.CreateMageOrScoutItems<MageItem>(scrapbookContent));
            Items.AddRange(itemProvider.CreateMageOrScoutItems<ScoutItem>(scrapbookContent));
        }
    }
}