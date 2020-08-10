using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Shares.Core.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

[assembly: InternalsVisibleTo("Core.Tests")]
namespace Shares.Core.Password
{
    internal class PasswordService : IPasswordService
    {
        private const byte FormatMarker = 0x01;
        private const KeyDerivationPrf Algorithm = KeyDerivationPrf.HMACSHA256;
        private readonly RandomNumberGenerator _rng;

        private readonly int _iterationCount;
        private readonly int _keySize;
        private readonly int _saltSize;

        public PasswordService(IConfiguration configuration)
        {
            var iterationCount = configuration.GetInt(Constants.PasswordIterationCountKey);
            if (iterationCount < 1)
            {
                throw new InvalidIterationCountException();
            }

            var keySize = configuration.GetInt(Constants.PasswordKeySizeKey);
            if (keySize % 8 != 0 || keySize / 8 < 1)
            {
                throw new InvalidKeySizeException();
            }

            var saltSize = configuration.GetInt(Constants.PasswordSaltSizeKey);
            if (saltSize % 8 != 0 || saltSize / 8 < 1)
            {
                throw new InvalidSaltSizeException();
            }
            
            _rng = new RNGCryptoServiceProvider();
            _iterationCount = iterationCount;
            _keySize = keySize / 8;
            _saltSize = saltSize / 8;
        }

        public string Hash(string pwd)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                throw new ArgumentNullException(nameof(pwd));
            }

            var salt = new byte[_saltSize];
            _rng.GetBytes(salt);
            var subKey = KeyDerivation.Pbkdf2(pwd, salt, Algorithm, _iterationCount, _keySize);

            var output = new byte[13 + _saltSize + _keySize];
            output[0] = FormatMarker;

            WriteHeader(output, 1, (uint)Algorithm);
            WriteHeader(output, 5, (uint) _iterationCount);
            WriteHeader(output, 9, (uint) _saltSize);

            Buffer.BlockCopy(salt, 0, output, 13, salt.Length);
            Buffer.BlockCopy(subKey, 0, output, 13+salt.Length, subKey.Length);

            return Convert.ToBase64String(output);

            static void WriteHeader(byte[] buf, int offset, uint value)
            {
                buf[offset + 0] = (byte)(value >> 24);
                buf[offset + 1] = (byte)(value >> 16);
                buf[offset + 2] = (byte)(value >> 8);
                buf[offset + 3] = (byte)(value >> 0);
            }
        }

        public bool Verify(string pwd, string base64Hash)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                throw new ArgumentNullException(nameof(pwd));
            }

            if (string.IsNullOrEmpty(base64Hash))
            {
                throw new ArgumentNullException(nameof(base64Hash));
            }

            try
            {
                var rawHash = Convert.FromBase64String(base64Hash);
                if (rawHash[0] != FormatMarker)
                {
                    return false;
                }

                var (alg, iterationCount, saltSize) = ScanHeader(rawHash);
                if (saltSize < _saltSize)
                {
                    return false;
                }

                var salt = new byte[saltSize];
                Buffer.BlockCopy(rawHash, 13, salt, 0, saltSize);

                var subKeyLength = rawHash.Length - 13 - saltSize;
                if (subKeyLength < _keySize)
                {
                    return false;
                }

                var expected = new byte[subKeyLength];
                Buffer.BlockCopy(rawHash, 13 + saltSize, expected, 0, subKeyLength);
                var actual = KeyDerivation.Pbkdf2(pwd, salt, alg, iterationCount, subKeyLength);

                return CryptographicOperations.FixedTimeEquals(actual, expected);
            }
            catch
            {
                // This shouldn't happen, unless the given hash was not originally hashing using this
                // service, i.e. the hash is in an invalid format or hashed using a third-party function.

                return false;
            }

            (KeyDerivationPrf Algorithm, int IterationCount, int SaltSize) ScanHeader(byte[] buf)
            {
                var alg = Algorithm;
                var iterationCount = _iterationCount;
                var saltSize = _saltSize;

                for (var i = 1; i < 13; i+= 4)
                {
                    var v = (((uint)buf[i]) << 24) | 
                            ((uint)(buf[i + 1]) << 16) |
                            (((uint)buf[i + 2]) << 8) | 
                            ((uint)buf[i + 3]);

                    switch (i)
                    {
                        case 1:
                            alg = (KeyDerivationPrf) v;
                            break;
                        case 5:
                            iterationCount = (int)v;
                            break;
                        case 9:
                            saltSize = (int)v;
                            break;
                    }
                }

                return (alg, iterationCount, saltSize);
            }
        }
    }
}
