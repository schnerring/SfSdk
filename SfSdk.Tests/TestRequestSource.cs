using System.Collections.Generic;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.DataSource;
using SfSdk.Response;

namespace SfSdk.Tests
{
    internal class TestRequestSource : IRequestSource
    {
        public async Task<SfResponse> RequestAsync(string sessionId, SF action, IEnumerable<string> args = null)
        {
            return new SfResponse(string.Empty);
        }
    }
}