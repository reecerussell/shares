using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Shares.Auth.Domain.Models;
using Shares.Core;
using Shares.Core.Entity;
using System.Threading.Tasks;

namespace Shares.Auth.Infrastructure.Providers
{
    internal class UserProvider : ReadRepository<User>
    {
        private readonly INormalizer _normalizer;

        public UserProvider(
            AuthContext context,
            INormalizer normalizer) 
            : base(context)
        {
            _normalizer = normalizer;
        }

        internal async Task<Maybe<User>> FindByEmailAsync(string email)
        {
            return await Set.SingleOrDefaultAsync(x => 
                x.NormalizedEmail == _normalizer.Normalize(email));
        }
    }
}
