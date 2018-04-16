﻿using System.Net.Http;
using FluentAssertions;
using Heine.Mvc.ActionFilters.Extensions;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.Extensions
{
    [TestFixture]
    public class HeaderUtilitiesTests
    {
        [Test]
        [TestCase("", ExpectedResult = "")]
        [TestCase("ASDASD", ExpectedResult = "AS****")]
        [TestCase("ASDASDASD123", ExpectedResult = "ASD*******...")]
        [TestCase("ASD ASDASD123", ExpectedResult = "ASD *********")]
        [TestCase("ASD ASDASD123123", ExpectedResult = "ASD **********...")]
        public string ObfuscateHeaders(string headerValue)
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", headerValue);
            var loggableHeaders = HeaderUtilities.GetLoggableHeaders(httpRequestMessage.Headers);
            return loggableHeaders.Should().ContainKey("Authorization").WhichValue;
        }
    }
}