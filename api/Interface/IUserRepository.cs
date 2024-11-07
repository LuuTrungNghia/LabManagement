using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dtos.User;
using api.Models;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(RegisterDto userDto);
        Task<UserDto> UpdateAsync(int id, UpdateUserRequestDto userDto);
        Task<bool> DeleteAsync(int id);
        Task<UserDto> ApproveUserAsync(int id);
        Task<UserDto> ResetPasswordAsync(int id, string newPassword);
        Task<IEnumerable<UserDto>> ImportUsersAsync(IEnumerable<CreateUserRequestDto> userDtos);
        Task<User> AuthenticateUserAsync(string username, string password);
        Task<User> GetByUsernameOrEmailAsync(string usernameOrEmail);
    }
}
