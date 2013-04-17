using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SfSdk.Constants;
using SfSdk.DataSource;
using SfSdk.Request;
using Xunit;

namespace SfSdk.Tests.Request
{
    public class SfRequestTests
    {
        [Fact]
        public void ExecuteAsyncThrowsExceptionIfSourceIsNull()
        {
            // Arrange
            var sut = new SfRequest();
            Func<Task> a = async () => await sut.ExecuteAsync(null, null, SF.ActAccountCreate);

            // Act / Assert
            a.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "source");
        }

        [Fact]
        public void ExecuteAsyncThrowsExceptionIfSessionIdIsNull()
        {
            // Arrange
            var sourceMock = new Mock<IRequestSource>();
            var sut = new SfRequest();
            Func<Task> a = async () => await sut.ExecuteAsync(sourceMock.Object, null, SF.ActAccountCreate);

            // Act / Assert
            a.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "sessionId");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfSessionIdHasInvalidLength()
        {
            // Arrange
            var sourceMock = new Mock<IRequestSource>();
            const string invalidSessionid = "000";
            var sut = new SfRequest();
            Func<Task> a = async () => await sut.ExecuteAsync(sourceMock.Object, invalidSessionid, SF.ActAccountCreate);

            // Act / Assert
            a.ShouldThrow<ArgumentException>()
             .Where(e => e.ParamName == "sessionId" && e.Message.StartsWith("SessionId must have a length of 32."));
        }
    }
}