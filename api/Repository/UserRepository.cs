using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.User;
using api.Interface;
using api.Mappers;
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

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _context.Users
                .Select(user => user.ToDto())
                .ToListAsync();
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user?.ToDto();
        }

        public async Task<UserDto> CreateAsync(CreateUserRequestDto userDto)
        {
            var user = userDto.ToModel();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.ToDto();
        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserRequestDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.UpdateModel(userDto);
            await _context.SaveChangesAsync();
            return user.ToDto();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
