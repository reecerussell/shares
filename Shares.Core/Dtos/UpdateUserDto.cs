using Shares.Core.Services;

namespace Shares.Core.Dtos
{
    public class UpdateUserDto
    {
        public UpdateUserDto()
        {
        }

        public UpdateUserDto(UpdateUserRequest request)
        {
            Id = request.Id;
            Firstname = request.Firstname;
            Lastname = request.Lastname;
            Email = request.Email;
        }

        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }

        public static implicit operator UpdateUserRequest(UpdateUserDto dto) => new UpdateUserRequest()
        {
            Id = dto.Id,
            Firstname = dto.Firstname,
            Lastname = dto.Lastname,
            Email = dto.Email,
        };
    }
}
