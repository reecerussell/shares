using Microsoft.AspNetCore.Http;
using Shares.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Shares.Web.Auth
{
    public class AuthorizedUser : IUser
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private IDictionary<string, object> _payload;

        public AuthorizedUser(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Id
        {
            get
            {
                if (_payload == null)
                {
                    PopulatePayload();
                }

                return _payload[ClaimTypes.UserId].ToString();
            }
        }

        private void PopulatePayload()
        {
            var token = _contextAccessor.HttpContext.Request.Headers
                .FirstOrDefault(x => x.Key
                    .Equals("authorization", StringComparison.OrdinalIgnoreCase))
                .Value[0][7..];
            var encodedPayload = token.Split('.')[1];
            var jsonData = Convert.FromBase64String(encodedPayload);
            _payload = JsonSerializer.Deserialize<IDictionary<string, object>>(jsonData);
        }
    }
}
