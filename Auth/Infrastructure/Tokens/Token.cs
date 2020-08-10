using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Shares.Core;
using Shares.Core.Extensions;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shares.Auth.Infrastructure.Tokens
{
    public class Token
    {
        public const string ExpiryKey = "exp";
        public const string IssuedAtKey = "iat";
        public const string NotBeforeKey = "nbf";

        public const string ExpiredMessage = "Token is expired.";
        public const string NotYetValidMessage = "Token is not yet valid.";

        public byte[] Data;
        private IDictionary<string, object> _payload;

        public Token(byte[] data)
        {
            Data = data;
        }

        private void ParsePayload()
        {
            if (_payload != null)
            {
                return;
            }

            var data = Encoding.UTF8.GetString(Data);
            var base64 = data.Split('.')[1];
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            _payload = JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
        }

        public DateTime? Expiry => GetDate(ExpiryKey);
        public double? ExpiryTimestamp => GetUnixTimestamp(ExpiryKey);
        public DateTime? IssuedAt => GetDate(IssuedAtKey);
        public double? IssuedAtTimestamp => GetUnixTimestamp(IssuedAtKey);
        public DateTime? NotBefore => GetDate(NotBeforeKey);
        public double? NotBeforeTimestamp => GetUnixTimestamp(NotBeforeKey);

        public DateTime? GetDate(string name)
        {
            var timestamp = GetUnixTimestamp(name);
            if (!timestamp.HasValue)
            {
                return null;
            }

            var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return date.AddMilliseconds(timestamp.Value);
        }

        public double? GetUnixTimestamp(string name)
        {
            ParsePayload();

            if (!_payload.ContainsKey(name))
            {
                return null;
            }

            if (!double.TryParse(_payload[name].ToString(), out var timestamp))
            {
                return null;
            }

            return timestamp;
        }

        public async Task<Result> VerifyAsync(IAsymmetricHasher hasher)
        {
            var (success, _, (data, signature), error) = Scan();
            if (!success)
            {
                return Result.Failure(error);
            }

            var result = await hasher.VerifyAsync(data, signature);
            if (result.IsFailure)
            {
                return result;
            }

            var now = DateTime.UtcNow.Unix();
            if (ExpiryTimestamp.HasValue && ExpiryTimestamp.Value <= now)
            {
                return Result.Failure(ExpiredMessage);
            }

            if (NotBeforeTimestamp.HasValue && NotBeforeTimestamp.Value > now)
            {
                return Result.Failure(NotYetValidMessage);
            }

            return Result.Success();
        }

        private Result<(byte[] Data, byte[] Signature)> Scan()
        {
            const byte separator = (byte)'.';
            var fd = Array.IndexOf(Data, separator);
            var ld = Array.LastIndexOf(Data, separator);

            if (ld <= fd)
            {
                return Result.Failure<(byte[] Data, byte[] Signature)>("Token is in an invalid format.");
            }

            var encodedLength = Data.Length - (ld + 1);
            var decodedLength = Base64.GetMaxDecodedFromUtf8Length(encodedLength);
            var buf = new byte[decodedLength];

            var status = Base64.DecodeFromUtf8(Data[(ld + 1)..], buf, out _, out var bytesWritten);
            if (status == OperationStatus.InvalidData)
            {
                return Result.Failure<(byte[] Data, byte[] Signature)>("Token signature is malformed.");
            }

            return (Data[..ld], buf[..bytesWritten]);
        }
    }
}
