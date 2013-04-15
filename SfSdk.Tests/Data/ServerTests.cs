using System;
using System.Threading.Tasks;
using SfSdk.Data;
using Xunit;

namespace SfSdk.Tests.Data
{
    public class ServerTests
    {
        [Fact]
        public async Task CreateServersAsyncThrowsExceptionIfCountryIsNull()
        {
            // Arrange / Act / Assert
            await TestHelpers.ThrowsAsync<ArgumentNullException>(async () => await Server.CreateServersAsync(null));
        } 
    }
}