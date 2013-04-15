using System;
using System.Threading.Tasks;
using SfSdk.Data;
using Xunit;

namespace SfSdk.Tests.Data
{
    public class CountryTests
    {
        private const string ValidUrl = "http://www.google.com/";

        [Fact]
        public async Task CreateAsyncThrowsNoExceptionWithValidParameters()
        {
            // Arrange / Act / Assert
            await TestHelpers.ThrowsNotAsync(async () => await Country.CreateAsync(null, new Uri(ValidUrl)));
        }

        [Fact]
        public async Task CreateAsyncThrowsExceptionIfUriIsNull()
        {
            // Arrange / Act / Assert
            await TestHelpers.ThrowsAsync<ArgumentNullException>(async () => await Country.CreateAsync(null, null));
        }
    }
}