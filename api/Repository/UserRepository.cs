using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Dtos.User;
using api.Interfaces;
using api.Models;
using api.Data;
using Microsoft.AspNetCore.Identity;

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User> AuthenticateUserAsync(string usernameOrEmail, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == usernameOrEmail.ToLower() || u.Email.ToLower() == usernameOrEmail.ToLower());

            if (user == null || !(await _userManager.CheckPasswordAsync(user, password)))
            {
                return null;
            }

            return user;
        }

        public async Task<UserDto> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == usernameOrEmail.ToLower() || u.Email.ToLower() == usernameOrEmail.ToLower());

            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role,
                Email = user.Email,
                IsApproved = user.IsApproved
            };
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
            string role = "User";

            var user = new User
            { 
                UserName = registerDto.Username, 
                Email = registerDto.Email, 
                Role = role, 
                IsApproved = false
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return null; // Handle errors if needed
            }

            return new UserDto 
            { 
                Id = user.Id, 
                Name = user.Name, 
                Role = user.Role, 
                Email = user.Email, 
                IsApproved = user.IsApproved 
            };
        }

        public async Task<UserDto> ApproveUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.IsApproved = true;
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

        public async Task<UserDto> ResetPasswordAsync(int id, string newPassword)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            var resetResult = await _userManager.RemovePasswordAsync(user);
            if (!resetResult.Succeeded) return null;

            var passwordResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (!passwordResult.Succeeded) return null;

            return new UserDto { Id = user.Id, Name = user.Name, Role = user.Role, Email = user.Email, IsApproved = user.IsApproved };
        }

        // Implement UpdateAsync
        public async Task<UserDto> UpdateAsync(int id, UpdateUserRequestDto updateUserRequestDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null; // Handle user not found
            }

            // Update user properties from the DTO
            user.Name = updateUserRequestDto.Name ?? user.Name;  // Use existing values if null
            user.Email = updateUserRequestDto.Email ?? user.Email;
            user.Role = updateUserRequestDto.Role ?? user.Role;
            user.IsApproved = updateUserRequestDto.IsApproved;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return null; // Handle errors in update
            }

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role,
                Email = user.Email,
                IsApproved = user.IsApproved
            };
        }

        // Implement DeleteAsync
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false; // Handle user not found
            }

            _context.Users.Remove(user);
            var result = await _context.SaveChangesAsync();

            return result > 0;  // Returns true if delete was successful, false if no rows were affected
        }
    }
}
