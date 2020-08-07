using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Shares.Users.ValueObjects
{
    public class UserFullname : ValueObject
    {
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }

        public string Fullname => string.Join(" ", Firstname, Lastname);

        internal Result Update(string firstname, string lastname)
        {
            if (string.IsNullOrEmpty(firstname))
            {
                return Result.Failure("Firstname cannot be empty.");
            }

            if (firstname.Length > 255)
            {
                return Result.Failure("Firstname cannot be greater than 255 characters long.");
            }

            if (string.IsNullOrEmpty(lastname))
            {
                return Result.Failure("Lastname cannot be empty.");
            }

            if (lastname.Length > 255)
            {
                return Result.Failure("Lastname cannot be greater than 255 characters long.");
            }

            Firstname = firstname;
            Lastname = lastname;

            return Result.Success();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Firstname;
            yield return Lastname;
        }

        internal static Result<UserFullname> Create(string firstname, string lastname)
        {
            var name = new UserFullname();
            return name.Update(firstname, lastname)
                .Map(() => name);
        }
    }
}
