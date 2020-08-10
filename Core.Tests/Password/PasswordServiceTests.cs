using Microsoft.Extensions.Configuration;
using Moq;
using Shares.Core.Password;
using System;
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

        [Fact]
        public void TestConstructor()
        {
            var config = BuildValidConfiguration();
            var service = new PasswordService(config);
            Assert.NotNull(service);
        }

        [Theory]
        [InlineData("P1ssw4rD")]
        [InlineData("Password123")]
        public void TestHash(string password)
        {
            var config = BuildValidConfiguration();
            var service = new PasswordService(config);
            var hash = service.Hash(password);
            Assert.NotNull(hash);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TestHashWithEmptyPassword(string password)
        {
            var config = BuildValidConfiguration();
            var service = new PasswordService(config);
            Assert.Throws<ArgumentNullException>(() => service.Hash(password));
        }

        [Theory]
        [InlineData("P1ssw4rD")]
        [InlineData("Password123")]
        public void TestVerify(string password)
        {
            var config = BuildValidConfiguration();
            var service = new PasswordService(config);
            var hash = service.Hash(password);

            Assert.True(service.Verify(password, hash));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TestVerifyWithEmptyPassword(string password)
        {
            var config = BuildValidConfiguration();
            var service = new PasswordService(config);
            Assert.Throws<ArgumentNullException>(() => service.Verify(password, null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TestVerifyWithEmptyHash(string hash)
        {
            var config = BuildValidConfiguration();
            var service = new PasswordService(config);
            Assert.Throws<ArgumentNullException>(() => service.Verify("TestPassword", hash));
        }

        private static IConfiguration BuildValidConfiguration()
        {
            var configProvider = new Mock<IConfiguration>();
            configProvider
                .SetupGet(x => x[Constants.PasswordIterationCountKey])
                .Returns(1500.ToString());
            configProvider
                .SetupGet(x => x[Constants.PasswordKeySizeKey])
                .Returns(256.ToString());
            configProvider
                .SetupGet(x => x[Constants.PasswordSaltSizeKey])
                .Returns(128.ToString());

            return configProvider.Object;
        }
    }
}
