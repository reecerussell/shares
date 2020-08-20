using Shares.Core.Extensions;
using System;
using Xunit;

namespace Shares.Core.Tests.Extensions
{
    public class DateTimeExtensionsTest
    {
        [Fact]
        public void TestUnix()
        {
            var time = new DateTime(2020, 8, 16);

            Assert.Equal(1597536000, time.Unix());
        }
    }
}
