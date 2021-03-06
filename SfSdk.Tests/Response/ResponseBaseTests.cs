﻿using System;
using FluentAssertions;
using SfSdk.Response;
using Xunit;

namespace SfSdk.Tests.Response
{
    public class ResponseBaseTests
    {
        private class TestResponse : ResponseBase
        {
            public TestResponse(string[] args) : base(args)
            {
            }
        }

        [Fact]
        public void ConstructorThrowsExceptionIfArgumentsAreNull()
        {
            // Arrange
            Action sut = () => new TestResponse(null);

            // Act / Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "args");
        }

        [Fact]
        public void ConstructorThrowsNoExceptionIfArgumentsAreNotNull()
        {
            // Arrange
            Action sut = () => new TestResponse(new string[0]);

            // Act / Assert
            sut.ShouldNotThrow<Exception>();
        }
    }
}