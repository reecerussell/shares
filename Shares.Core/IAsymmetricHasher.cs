using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace Shares.Core
{
    public interface IAsymmetricHasher
    {
        string HashName { get; }
        Task<Result<byte[]>> SignAsync(byte[] hash);
        Task<Result> VerifyAsync(byte[] message, byte[] signature);
    }
}
