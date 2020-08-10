using Shares.Core.Entity;

namespace Shares.Auth.Domain.Models
{
    public class User : Aggregate
    {
        public string NormalizedEmail { get; private set; }
        public string Hash { get; private set; }
    }
}
