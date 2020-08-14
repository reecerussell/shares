using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace Shares.Core
{
    public interface IAsymmetricHasher
    {
        string HashName { get; }
        Task<Result<byte[]>> SignAsync(byte[] digest);
        Task<Result> VerifyAsync(byte[] digest, byte[] signature);
    }
}
