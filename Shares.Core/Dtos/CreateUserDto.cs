using Shares.Core.Services;

namespace Shares.Core.Dtos
{
    public class CreateUserDto
    {
        public CreateUserDto()
        {
        }

        public CreateUserDto(CreateUserRequest request)
        {
            Firstname = request.Firstname;
            Lastname = request.Lastname;
            Email = request.Email;
            Password = request.Password;
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public static implicit operator CreateUserRequest(CreateUserDto dto) => new CreateUserRequest
        {
            Firstname = dto.Firstname,
            Lastname = dto.Lastname,
            Email = dto.Email,
            Password = dto.Password
        };
    }
}
