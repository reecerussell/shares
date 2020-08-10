using CSharpFunctionalExtensions;
using Shares.Auth.Infrastructure.Tokens;
using Shares.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Infrastructure.Test.Tokens
{
    public class TokenBuilderTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TokenBuilderTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestConstructor()
        {
            const string alg = "RSA256";
            var builder = new TokenBuilder(alg);

            Assert.NotNull(builder.Header);
            Assert.Equal(alg, builder.Header.Algorithm);
            Assert.Equal("JWT", builder.Header.Type);
            Assert.NotNull(builder.Claims);
        }

        [Theory]
        [InlineData("Hello", "World")]
        public void TestAddClaim(string name, object value)
        {
            var builder = new TokenBuilder("RSA256");
            builder.AddClaim(name, value);

            Assert.Equal(1, builder.Claims.Count);
            Assert.Equal(value, builder.Claims[name]);
        }

        [Fact]
        public void TestAddClaims()
        {
            var claims = new Dictionary<string, object>
            {
                {"Number", 0},
                {"String", "Hello World"},
                {"Boolean", true}
            };

            var builder = new TokenBuilder("RSA256")
                .AddClaims(claims);

            Assert.Equal(claims.Count, builder.Claims.Count);

            foreach (var (key, value) in claims)
            {
                Assert.Equal(value, builder.Claims[key]);
            }
        }

        [Fact]
        public void TestSetExpiry()
        {
            var time = DateTime.Now.AddDays(1);
            var builder = new TokenBuilder("RSA256")
                .SetExpiry(time);

            var value = builder.Claims[Token.ExpiryKey];
            Assert.Equal(time.Unix(), value);
        }

        [Fact]
        public void TestSetIssuedAt()
        {
            var time = DateTime.Now.AddDays(1);
            var builder = new TokenBuilder("RSA256")
                .SetIssuedAt(time);

            var value = builder.Claims[Token.IssuedAtKey];
            Assert.Equal(time.Unix(), value);
        }

        [Fact]
        public void TestSetNotBefore()
        {
            var time = DateTime.Now.AddDays(1);
            var builder = new TokenBuilder("RSA256")
                .SetNotBefore(time);

            var value = builder.Claims[Token.NotBeforeKey];
            Assert.Equal(time.Unix(), value);
        }

        [Fact]
        public void TestBuild()
        {
            var hasher = new TestHasher();
            var builder = new TokenBuilder(hasher.HashName)
                .AddClaim("Hello", "World");
            var (success, _, token, error) = builder.BuildAsync(hasher).Result;
            Assert.True(success, error);

            _testOutputHelper.WriteLine(Encoding.UTF8.GetString(token));

            (success, _, error) = new Token(token).VerifyAsync(hasher).Result;
            Assert.True(success, error);
        }
    }
}
