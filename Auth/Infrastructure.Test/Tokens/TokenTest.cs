using CSharpFunctionalExtensions;
using Shares.Auth.Infrastructure.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.Test.Tokens
{
    public class TokenTest
    {
        private readonly TestHasher _testHasher;

        public TokenTest()
        {
            _testHasher = new TestHasher();
        }

        [Fact]
        public void TestConstructor()
        {
            var data = Encoding.UTF8.GetBytes("Hello World");
            var token = new Token(data);
            Assert.Equal(data, token.Data);
        }

        [Fact]
        public async Task TestValidVerify()
        {
            var exp = DateTime.UtcNow.AddHours(1);
            var iat = DateTime.UtcNow;
            var builder = new TokenBuilder(_testHasher.HashName)
                .SetExpiry(exp)
                .SetIssuedAt(iat)
                .SetNotBefore(iat);

            var (success, _, tokenData, error) = await builder.BuildAsync(_testHasher);
            Assert.True(success, error);

            var token = new Token(tokenData);
            (success, _, error) = await token.VerifyAsync(_testHasher);
            Assert.True(success, error);
        }

        [Fact]
        public async Task TestExpiryVerify()
        {
            var exp = DateTime.UtcNow.AddHours(-1);
            var iat = DateTime.UtcNow;
            var builder = new TokenBuilder(_testHasher.HashName)
                .SetExpiry(exp)
                .SetIssuedAt(iat)
                .SetNotBefore(iat);

            var (success, _, tokenData, error) = await builder.BuildAsync(_testHasher);
            Assert.True(success, error);

            var token = new Token(tokenData);
            (success, _, error) = await token.VerifyAsync(_testHasher);
            Assert.False(success);
            Assert.Equal(Token.ExpiredMessage, error);
        }

        [Fact]
        public async Task TestNotYetValidVerify()
        {
            var exp = DateTime.UtcNow.AddHours(1);
            var iat = DateTime.UtcNow;
            var builder = new TokenBuilder(_testHasher.HashName)
                .SetExpiry(exp)
                .SetIssuedAt(iat)
                .SetNotBefore(iat.AddHours(1));

            var (success, _, tokenData, error) = await builder.BuildAsync(_testHasher);
            Assert.True(success, error);

            var token = new Token(tokenData);
            (success, _, error) = await token.VerifyAsync(_testHasher);
            Assert.False(success);
            Assert.Equal(Token.NotYetValidMessage, error);
        }
    }
}