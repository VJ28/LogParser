using System;
using Xunit;
using System.IO;

namespace LogParser.Tests
{
    public class Utility_Test
    {
        [Fact]
        public void Should_Throw_Exception_For_Invalid_URL()
        {
            Assert.Throws<ArgumentException>(() => Utility.ReplaceIdInURL(""));
        }
        [Theory]
        [InlineData("/api/home")]
        [InlineData("/api/v2/34")]
        [InlineData("/api/v2/34/details")]
        public void Should_Return_Modified_Url(string url)
        {
            var outputUrl = Utility.ReplaceIdInURL(url);
            Assert.Equal(url.Replace("34", "{id}"), outputUrl);
        }
    }
}