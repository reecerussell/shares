using Amazon.KeyManagementService;
using Amazon.Runtime.CredentialManagement;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shares.AWS;
using Shares.Core;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Test
{
    public class AsymmetricHasherTest
    {
        private readonly IAsymmetricHasher _service;

        public AsymmetricHasherTest()
        {
            var credentialsFile = new SharedCredentialsFile("../../../../../credentials");
            if (!credentialsFile.TryGetProfile("Shares", out var profile))
            {
                throw new InvalidOperationException("cannot get shares profile");
            }

            if (!AWSCredentialsFactory.TryGetAWSCredentials(profile, credentialsFile, out var credential))
            {
                throw new InvalidOperationException("cannot get credentials");
            }

            var logger = new NullLogger<AsymmetricHasher>();
            var retriever = new Mock<IConfiguration>();
            retriever.Setup(x => x[Constants.HasherKeyIdKey]).Returns("alias/shares-jwt");
            var config = retriever.Object;

            _service = new AsymmetricHasher(
                new AmazonKeyManagementServiceClient(credential),
                logger,
                config);
        }

        [Fact]
        public async Task TestSign()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello World!");
            using var alg = SHA256.Create();
            var digest = alg.ComputeHash(bytes);

            var (success, _, _, error) = await _service.SignAsync(digest);
            Assert.True(success, error);
        }

        [Fact]
        public async Task TestSignWithInvalidDigest()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello World!");

            var result = await _service.SignAsync(bytes);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task TestVerify()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello World!");
            using var alg = SHA256.Create();
            var digest = alg.ComputeHash(bytes);

            var (success, _, signature, error) = await _service.SignAsync(digest);
            Assert.True(success, error);

            (success, _, error) = await _service.VerifyAsync(digest, signature);
            Assert.True(success, error);
        }

        [Fact]
        public async Task TestVerifyWithMalformedSignature()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello World!");
            using var alg = SHA256.Create();
            var digest = alg.ComputeHash(bytes);

            var (success, _, _, error) = await _service.SignAsync(digest);
            Assert.True(success, error);

            var result = await _service.VerifyAsync(digest, bytes);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task TestVerifyWithInvalidSignature()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello World!");
            using var alg = SHA256.Create();
            var digest = alg.ComputeHash(bytes);

            var (success, _, signature, error) = await _service.SignAsync(digest);
            Assert.True(success, error);

            digest = alg.ComputeHash(Encoding.UTF8.GetBytes("Goodbye!"));
            var result = await _service.VerifyAsync(digest, signature);
            Assert.False(result.IsSuccess);
        }
    }
}
