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

        [Fact]
        public void TestVerifyWithInvalidFormatMarker()
        {
            const string testPassword = "TestPassword";
            var config = BuildValidConfiguration();
            var service = new PasswordService(config);
            var hash = Convert.FromBase64String(service.Hash(testPassword));
            
            hash[0] = 0x02; // change format marker

            var ok = service.Verify(testPassword, Convert.ToBase64String(hash));
            Assert.False(ok);
        }

        [Fact]
        public void TestVerifyWithInvalidSaltSize()
        {
            const string testPassword = "TestPassword";
            var config = BuildValidConfiguration();
            var service = new PasswordService(config);
            var hash = Convert.FromBase64String(service.Hash(testPassword));

            // Change salt size to be smaller than password service's salt size.
            var masterSaltSize = int.Parse(config[Constants.PasswordSaltSizeKey]);
            var newSaltSize = masterSaltSize - 1;
            hash[9] = (byte) (newSaltSize >> 24);
            hash[10] = (byte)(newSaltSize >> 16);
            hash[11] = (byte)(newSaltSize >> 8);
            hash[12] = (byte)(newSaltSize >> 0);

            var ok = service.Verify(testPassword, Convert.ToBase64String(hash));
            Assert.False(ok);
        }

        [Theory]
        [InlineData("AQAAAAAAAAXcAAAAEPf5Otm47kJ/2TISVxfdfLRD9makOnv9qFBaCixu3lPN3j2mEGk8OLNgzr1m6z+dSA==")] // HMACSHA1
        [InlineData("AQAAAAIAAAXcAAAAEPf5Otm47kJ/2TISVxfdfLRD9makOnv9qFBaCixu3lPN3j2mEGk8OLNgzr1m6z+dSA==")] // HMACSHA512
        public void TestVerifyWithMismatchHashAlgorithm(string invalidHash)
        {
            var config = BuildValidConfiguration();
            var service = new PasswordService(config);

            const string testPassword = "TestPassword";
            var ok = service.Verify(testPassword, invalidHash);
            Assert.False(ok);
        }

        [Theory]
        [InlineData("this is not base64")]
        [InlineData("AQAAAAMAAAXcAAAAEPf5Otm47kJ/2TISVxfdfLRD9makOnv9qFBaCixu3lPN3j2mEGk8OLNgzr1m6z+dSA==")]
        public void TestVerifyWithInvalidHash(string hash)
        {
            var config = BuildValidConfiguration();
            var service = new PasswordService(config);

            var ok = service.Verify("TestPassword", hash);
            Assert.False(ok);
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
