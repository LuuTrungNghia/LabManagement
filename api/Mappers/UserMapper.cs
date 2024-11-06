using api.Dtos.User;
using api.Models;

namespace api.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role,
                Email = user.Email,
            };
        }

        public static User ToModel(this CreateUserRequestDto dto)
        {
            return new User
            {
                Name = dto.Name,
                Role = dto.Role,
                Email = dto.Email
            };
        }

        public static void UpdateModel(this User user, UpdateUserRequestDto dto)
        {
            user.Name = dto.Name;
            user.Role = dto.Role;
            user.Email = dto.Email;
        }
    }
}
