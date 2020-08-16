using Shares.Core.Services;

namespace Shares.Core.Dtos
{
    public class ChangePasswordDto
    {
        public ChangePasswordDto()
        {
        }

        public ChangePasswordDto(ChangePasswordRequest request)
        {
            Id = request.Id;
            CurrentPassword = request.CurrentPassword;
            NewPassword = request.NewPassword;
        }

        public string Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        public static implicit operator ChangePasswordRequest(ChangePasswordDto dto) => new ChangePasswordRequest
        {
            Id = dto.Id,
            CurrentPassword = dto.CurrentPassword,
            NewPassword = dto.NewPassword
        };
    }
}
