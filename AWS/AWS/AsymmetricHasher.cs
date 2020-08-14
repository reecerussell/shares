using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shares.Core;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("AWS.Test")]
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
        public async Task<Result<byte[]>> SignAsync(byte[] digest)
        {
            var request = new SignRequest
            {
                KeyId = _keyId,
                Message = new MemoryStream(digest),
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

        public async Task<Result> VerifyAsync(byte[] digest, byte[] signature)
        {
            var request = new VerifyRequest
            {
                KeyId = _keyId,
                Message = new MemoryStream(digest),
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
            catch (KMSInternalException e)
            {
                _logger.LogError(e, "An internal error occured with AWS KMS: {0}", e.Message);

                return Result.Failure("An internal error occured while verify the hash.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return Result.Failure<byte[]>(e.Message);
            }
        }
    }
}
