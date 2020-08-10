using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Shares.Core;
using Shares.Core.Extensions;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shares.Auth.Infrastructure.Tokens
{
    public class TokenBuilder
    {
        public readonly TokenHeader Header;
        public readonly IDictionary<string, object> Claims;

        public TokenBuilder(string alg)
        {
            Header = new TokenHeader(alg, "JWT");
            Claims = new Dictionary<string, object>();
        }

        public TokenBuilder AddClaim(string name, object value)
        {
            Claims[name] = value;

            return this;
        }

        public TokenBuilder AddClaims(IDictionary<string, object> claims)
        {
            foreach (var (name, value) in claims)
            {
                Claims[name] = value;
            }

            return this;
        }

        /// <summary>
        /// Sets the "Expiry" claim (exp) to the given time, in the form
        /// of the number of milliseconds since 1970-01-01T00:00:00Z UTC, ignoring leap seconds.
        /// </summary>
        /// <param name="time">The <see cref="DateTime"/> value for the Expiry claim.</param>
        /// <returns>Returns the current <see cref="TokenBuilder"/></returns>
        public TokenBuilder SetExpiry(DateTime time)
        {
            return AddClaim(Token.ExpiryKey, time.Unix());
        }

        /// <summary>
        /// Sets the "Issued At" claim (iat) to the given time, in the form
        /// of the number of milliseconds since 1970-01-01T00:00:00Z UTC, ignoring leap seconds.
        /// </summary>
        /// <param name="time">The <see cref="DateTime"/> value for the Issued At claim.</param>
        /// <returns>Returns the current <see cref="TokenBuilder"/></returns>
        public TokenBuilder SetIssuedAt(DateTime time)
        {
            return AddClaim(Token.IssuedAtKey, time.Unix());
        }

        /// <summary>
        /// Sets the "Not Before" claim (nbf) to the given time, in the form
        /// of the number of milliseconds since 1970-01-01T00:00:00Z UTC, ignoring leap seconds.
        /// </summary>
        /// <param name="time">The <see cref="DateTime"/> value for the Not Before claim.</param>
        /// <returns>Returns the current <see cref="TokenBuilder"/></returns>
        public TokenBuilder SetNotBefore(DateTime time)
        {
            return AddClaim(Token.NotBeforeKey, time.Unix());
        }

        public async Task<Result<byte[]>> BuildAsync(IAsymmetricHasher hasher)
        {
            var header = Serialize(Header);
            var payload = Serialize(Claims);
            var data = new byte[header.Length + 1 + payload.Length];

            Buffer.BlockCopy(header, 0, data, 0, header.Length);
            data[header.Length] = (byte) '.';
            Buffer.BlockCopy(payload, 0, data, header.Length + 1, payload.Length);

            var alg = SHA256.Create();
            var digest = alg.ComputeHash(data);

            var result = await hasher.SignAsync(digest);
            if (result.IsFailure)
            {
                return result;
            }

            var signature = result.Value;
            var base64Signature = new byte[Base64.GetMaxEncodedToUtf8Length(signature.Length)];
            Base64.EncodeToUtf8(signature, base64Signature, out _, out _);

            var token = new byte[data.Length + 1 + base64Signature.Length];

            Buffer.BlockCopy(data, 0, token, 0, data.Length);
            token[data.Length] = (byte) '.';
            Buffer.BlockCopy(base64Signature, 0, token, data.Length + 1, base64Signature.Length);

            return token;

            static byte[] Serialize(object value)
            {
                var json = JsonConvert.SerializeObject(value);
                var bytes = Encoding.UTF8.GetBytes(json);
                var base64 = Convert.ToBase64String(bytes);
                return Encoding.UTF8.GetBytes(base64);
            }
        }
    }

    public class TokenHeader
    {
        [JsonProperty("alg")]
        public string Algorithm { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public TokenHeader(string alg, string typ)
        {
            Algorithm = alg;
            Type = typ;
        }
    }
}
