using CSharpFunctionalExtensions;

namespace Shares.Users.Services
{
    public interface IPasswordService
    {
        string Hash(string pwd);
        bool Verify(string pwd, string hash);
        Result Validate(string pwd);
    }
}
