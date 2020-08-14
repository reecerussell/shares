using Microsoft.AspNetCore.Mvc;

namespace Shares.Web.Auth
{
    public class ValidateTokenAttribute : ServiceFilterAttribute
    {
        public ValidateTokenAttribute() : base(typeof(AuthorizationFilter))
        {
        }
    }
}
