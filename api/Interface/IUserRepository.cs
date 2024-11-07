using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dtos.User;
using api.Models;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AuthenticateUserAsync(string usernameOrEmail, string password);
        Task<UserDto> GetByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> CreateAsync(RegisterDto registerDto);
        Task<UserDto> UpdateAsync(int id, UpdateUserRequestDto updateUserDto);
        Task<bool> DeleteAsync(int id);
        Task<UserDto> ApproveUserAsync(int id);
        Task<UserDto> ResetPasswordAsync(int id, string newPassword);
        Task<UserDto> GetByUsernameOrEmailAsync(string usernameOrEmail);
    }
}

