using CSharpFunctionalExtensions;

namespace Shares.Core
{
    public interface IPasswordValidator
    {
        Result Validate(string password);
    }
}
