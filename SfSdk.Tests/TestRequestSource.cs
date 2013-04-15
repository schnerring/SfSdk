using System;
using System.Threading.Tasks;
using SfSdk.DataSource;
using SfSdk.Request;
using SfSdk.Response;

namespace SfSdk.Tests
{
    internal class TestRequestSource : RequestSourceBase
    {
        public TestRequestSource(SnFUriFactory uriFactory) : base(uriFactory)
        {
        }

        public override async Task<SfResponse> RequestAsync()
        {
            return new SfResponse(string.Empty);
        }
    }
}