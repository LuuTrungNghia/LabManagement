using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.User;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == usernameOrEmail.ToLower() || u.Email.ToLower() == usernameOrEmail.ToLower());
            return user;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync() =>
            await _context.Users.Select(u => new UserDto { Id = u.Id, Name = u.Name, Role = u.Role, Email = u.Email, IsApproved = u.IsApproved }).ToListAsync();

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? null : new UserDto { Id = user.Id, Name = user.Name, Role = user.Role, Email = user.Email, IsApproved = user.IsApproved };
        }

        public async Task<UserDto> CreateAsync(RegisterDto registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.Role)) 
            {
                registerDto.Role = "User";
            }

            var hashedPassword = HashPassword(registerDto.Password);

            var user = new User 
            { 
                Name = registerDto.Username, 
                Email = registerDto.Email, 
                Password = hashedPassword,
                Role = registerDto.Role,
                IsApproved = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto 
            { 
                Id = user.Id, 
                Name = user.Name, 
                Role = user.Role, 
                Email = user.Email, 
                IsApproved = user.IsApproved 
            };
        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserRequestDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.Name = updateUserDto.Name;
            user.Role = updateUserDto.Role;
            user.Email = updateUserDto.Email;
            user.IsApproved = updateUserDto.IsApproved;
            await _context.SaveChangesAsync();

            return new UserDto { Id = user.Id, Name = user.Name, Role = user.Role, Email = user.Email, IsApproved = user.IsApproved };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto> ApproveUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.IsApproved = true;
            await _context.SaveChangesAsync();
            return new UserDto { Id = user.Id, Name = user.Name, Role = user.Role, Email = user.Email, IsApproved = user.IsApproved };
        }

        public async Task<UserDto> ResetPasswordAsync(int id, string newPassword)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.Password = HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return new UserDto { Id = user.Id, Name = user.Name, Role = user.Role, Email = user.Email, IsApproved = user.IsApproved };
        }

        public async Task<IEnumerable<UserDto>> ImportUsersAsync(IEnumerable<CreateUserRequestDto> userDtos)
        {
            var users = userDtos.Select(dto => new User { Name = dto.Name, Role = dto.Role, Email = dto.Email, Password = HashPassword(dto.Password), IsApproved = false });
            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();
            return users.Select(u => new UserDto { Id = u.Id, Name = u.Name, Role = u.Role, Email = u.Email, IsApproved = u.IsApproved });
        }

        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null; // Authentication failed
            }
            return user;
        }

        private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    }
}
