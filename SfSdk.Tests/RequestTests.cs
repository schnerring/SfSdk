using System;
using SfSdk.Enums;
using Xunit;

namespace SfSdk.Tests
{
    public class RequestTests
    {
        [Fact]
        public void Test1()
        {
            var a = new Request("asdf", new Uri("http://www.google.de"), SfAction.Album);
        }
    }
}