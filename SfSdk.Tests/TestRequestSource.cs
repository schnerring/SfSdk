using System.Collections.Generic;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Request;
using SfSdk.Response;

namespace SfSdk.Tests
{
    internal class TestRequestSource : IRequestSource
    {
#pragma warning disable 1998
        public async Task<ISfResponse> RequestAsync(string sessionId, SF action, IEnumerable<string> args = null)
#pragma warning restore 1998
        {
            return new SfResponse(string.Empty);
        }
    }
}