using CSharpFunctionalExtensions;
using Shares.Core;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Infrastructure.Test.Tokens
{
    internal class TestHasher : IAsymmetricHasher
    {
        private const string PrivateKeyData = @"MIIEpAIBAAKCAQEAxvS8FpGulNtI03h1qdZhj6dTRUaXf6bTXI9fy5uwsSh8FfHA
Ja8v2+pjk4X1LyosQjeQU0mlKQG+Koj7rAbi/XROUbcF0DX3jkXxzTY+x2mMWXEU
FN29G9fX23h+yqf+iAAVpS7fyNl1k4ZlW0HqjsR1WhDnB/grw5oza1cXImNXwJMT
JrmN3VImjsTCUTf5hwpe/fVnAMkp2qz0sNUDBrACibT1uBTqS8e7HYIH8VbNN5PA
uue525Z42Cdvauk56lmdAeYtzgoj+mzhlM3XMneNfCICFMlQ6VVMxErxODN9t8cN
sOY/9wfgZ9B23MYVUsX24hXV6WIhR8uuU4duXQIDAQABAoIBACQg+yrqB5b2fzIP
tOZDjPHmQ8PXvtkqdUVgr9esjRiOroHZpbfyKTG5TZ3TfKO099aKeoX1YK2iqGLW
/0TM91lpTnOeVcrqkerfHrrSN/JEY302gJwNbDLrLkKnjfbFLP6LbWLSsUaq3qQM
islYnfyRBnJ1kU6sLV5W2TGL3iKbL66uXzC3GlUQwfc/ABs0krFmDi6IM0KwfDkL
xP9q4oAYYj/pbjHkz2LIZQbVJvFgFtXWgxSotY+/cKU8CdaEdWJDnTrhKRrRTCdD
Mm+eSW3nehoxjiVG0X73cWY4KZ22D/i9nG3EbkVvk74PdeQSQiXHNdP236MOR5aE
jyw1/KECgYEA5tjeuMQ9BzuO7dDqr6/R5d4MZwkBA3wxmKCB2JabEyYN+RpvlOJI
G972nPxGkgWmjvAlA6T/k0m2xvR9zajS8aj6xScRG/LS5pvbphVN5o1v7wstdAvb
4x3k7f53ZtO+lHn+DEMgsQOMmqw3NZ5booNlZ/7SgsLJaTbS+GTxRKkCgYEA3KJP
O8tjZng5Kig+ttbnKk/FS/GiPlQBL+wmHkri4nSFrZ7GEzecCQz9sTG6Kf9pxPWv
H0dX2ftG1KhiuxB8zgl2ximNrP4egMAs/6MxMB5wT9OL8MXmjOHlxK06NxV0Vk4p
83tOFiu8S7L37MfuXpbRTgHmma+qR4w0JtNZuJUCgYEA08bjVIgbILZ/4iaGFtTq
b8IsiJ3XAzZ9XXLqjCNmcsO3j+6zrNeGpaL/hXde2nk6mukuW9CT8rBucMk9XF3j
33VRUWsMpCI5XvxfHldvFvJsXNVZRpvWht4W9ks7iOb6cMYVzXQL2rjR1dfl8ler
40Q91JEO2I0QL2jvVJduS7kCgYEAmLT+gjRTqCit6C/SuFolxXNQ6y9jTCB4ceLQ
v0a1omdj3rd2APcMWHdVX787SrYtTtV9T/jhJU51x9qD+1V+DF0giCdal9GK6zIX
8xfQg62NrrKMuSlAWWJA1c/P6zA+RT30a0F82nuK6BoG3yvCIFXiqEtIgDbFdJ/l
uLmv78ECgYBjaE1qWFLGr/Z6JqKo/EuuOgRh+LOk8QVphRbDzyKvtsTyAynmhKvM
+PA4bZoibZ8INMjEnvI8t5JknUvdIHf7i+BIy/hLu1xOsG391/MYo6LBHdQ1Fp8i
SkqQDlfc+ppWv1RAiFj18rG9+Gel0+CJl2LqHdZu4rg6tc3nvQ/oRQ==";

        private readonly RSA _rsa;
        private readonly HashAlgorithmName _hashAlgorithm;

        public TestHasher()
        {
            var bytes = Convert.FromBase64String(PrivateKeyData);
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(bytes, out _);
            _rsa = rsa;
            _hashAlgorithm = HashAlgorithmName.SHA256;
        }

        public string HashName => "RSA256";

        public async Task<Result<byte[]>> SignAsync(byte[] hash)
        {
            return _rsa.SignHash(hash, _hashAlgorithm, RSASignaturePadding.Pkcs1);
        }

        public async Task<Result> VerifyAsync(byte[] message, byte[] signature)
        {
            var ok = _rsa.VerifyHash(message, signature, _hashAlgorithm, RSASignaturePadding.Pkcs1);
            if (ok)
            {
                return Result.Success();
            }

            return Result.Failure("VerifyAsync: false");
        }
    }
}
