using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shares.Core;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Shares.AWS
{
    internal class AsymmetricHasher : IAsymmetricHasher
    {
        private readonly IAmazonKeyManagementService _service;
        private readonly ILogger<AsymmetricHasher> _logger;
        private readonly string _keyId;

        public AsymmetricHasher(
            IAmazonKeyManagementService service,
            ILogger<AsymmetricHasher> logger,
            IConfiguration configuration)
        {
            _service = service;
            _logger = logger;
            _keyId = configuration[Constants.HasherKeyIdKey];

            if (string.IsNullOrEmpty(_keyId))
            {
                throw new InvalidOperationException($"No key id was found in '{Constants.HasherKeyIdKey}'.");
            }
        }

        public string HashName => "RSA256";
        public async Task<Result<byte[]>> SignAsync(byte[] hash)
        {
            var request = new SignRequest
            {
                KeyId = _keyId,
                Message = new MemoryStream(hash),
                MessageType = MessageType.DIGEST,
                SigningAlgorithm = SigningAlgorithmSpec.RSASSA_PKCS1_V1_5_SHA_256
            };

            try
            {
                var response = await _service.SignAsync(request);
                return response.Signature.ToArray();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return Result.Failure<byte[]>(e.Message);
            }
        }

        public async Task<Result> VerifyAsync(byte[] message, byte[] signature)
        {
            var request = new VerifyRequest
            {
                KeyId = _keyId,
                Message = new MemoryStream(message),
                MessageType = MessageType.DIGEST,
                Signature = new MemoryStream(signature),
                SigningAlgorithm = SigningAlgorithmSpec.RSASSA_PKCS1_V1_5_SHA_256
            };

            try
            {
                var response = await _service.VerifyAsync(request);
                if (!response.SignatureValid)
                {
                    throw new KMSInvalidSignatureException("Invalid signature");
                }

                return Result.Success();
            }
            catch (KMSInvalidSignatureException)
            {
                return Result.Failure("Invalid signature.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return Result.Failure<byte[]>(e.Message);
            }
        }

        public async Task<int> GetPublicKeySizeAsync()
        {
            var request = new GetPublicKeyRequest
            {
                KeyId = _keyId,
            };

            var response = await _service.GetPublicKeyAsync(request);
            await using var keyStream = response.PublicKey;
            using var key = RSA.Create();
            key.ImportRSAPublicKey(keyStream.ToArray(), out _);
            
            return key.KeySize;
        }
    }
}
