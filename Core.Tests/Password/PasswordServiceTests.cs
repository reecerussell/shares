using Microsoft.Extensions.Configuration;
using Moq;
using Shares.Core.Password;
using Xunit;

namespace Shares.Core.Tests.Password
{
    public class PasswordServiceTests
    {
        [Theory]
        [InlineData(0)]
        public void TestConstructorWithInvalidIterationCount(int count)
        {
            var configProvider = new Mock<IConfiguration>();
            configProvider
                .SetupGet(x => x[Constants.PasswordIterationCountKey])
                .Returns(count.ToString);

            var config = configProvider.Object;

            Assert.Equal(count, int.Parse(config[Constants.PasswordIterationCountKey]));
            Assert.Throws<InvalidIterationCountException>(() => { new PasswordService(config); });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-8)]
        [InlineData(5)]
        public void TestConstructorWithInvalidKeySize(int size)
        {
            var configProvider = new Mock<IConfiguration>();
            configProvider
                .SetupGet(x => x[Constants.PasswordIterationCountKey])
                .Returns("1500");
            configProvider
                .SetupGet(x => x[Constants.PasswordKeySizeKey])
                .Returns(size.ToString);

            var config = configProvider.Object;

            Assert.Equal(size, int.Parse(config[Constants.PasswordKeySizeKey]));
            Assert.Throws<InvalidKeySizeException>(() => { new PasswordService(config); });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-8)]
        [InlineData(5)]
        public void TestConstructorWithInvalidSaltSize(int size)
        {
            var configProvider = new Mock<IConfiguration>();
            configProvider
                .SetupGet(x => x[Constants.PasswordIterationCountKey])
                .Returns("1500");
            configProvider
                .SetupGet(x => x[Constants.PasswordKeySizeKey])
                .Returns("256");
            configProvider
                .SetupGet(x => x[Constants.PasswordSaltSizeKey])
                .Returns(size.ToString);

            var config = configProvider.Object;

            Assert.Equal(size, int.Parse(config[Constants.PasswordSaltSizeKey]));
            Assert.Throws<InvalidSaltSizeException>(() => { new PasswordService(config); });
        }
    }
}
