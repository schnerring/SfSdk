using System;
using System.Threading.Tasks;
using FluentAssertions;
using SfSdk.DataSource;
using SfSdk.Request;
using SfSdk.Response;
using Xunit;

namespace SfSdk.Tests.DataSource
{
    public class RequestSourceBaseTests
    {
        private class TestRequestSource : RequestSourceBase
        {
            public TestRequestSource(IUriFactory uriFactory) : base(uriFactory)
            {
            }

            public override Task<SfResponse> RequestAsync()
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void ConstructorThrowsExceptionIfUriFactoryIsNull()
        {
            // Arrange / Act
            Action a = () => new TestRequestSource(null);

            // Assert
            a.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "uriFactory");
        }
    }
}