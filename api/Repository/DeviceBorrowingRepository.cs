using api.Data;
using api.Dtos.DeviceBorrowing;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class DeviceBorrowingRepository : IDeviceBorrowingRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceBorrowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeviceBorrowing?> GetByIdAsync(int id) => 
            await _context.DeviceBorrowings.FindAsync(id);

        public async Task<IEnumerable<DeviceBorrowing>> GetByUserIdAsync(int userId) =>
            await _context.DeviceBorrowings.Where(b => b.UserId == userId).ToListAsync();

        public async Task CreateAsync(DeviceBorrowing borrowing)
        {
            await _context.DeviceBorrowings.AddAsync(borrowing);
            await _context.SaveChangesAsync();
        }

        public async Task<DeviceBorrowing?> UpdateAsync(int id, UpdateDeviceBorrowingRequestDto borrowingDto)
        {
            var borrowing = await GetByIdAsync(id);
            if (borrowing == null) return null;

            borrowing.StartDate = borrowingDto.StartDate;
            borrowing.EndDate = borrowingDto.EndDate;
            borrowing.BorrowerType = borrowingDto.BorrowerType;

            _context.DeviceBorrowings.Update(borrowing);
            await _context.SaveChangesAsync();
            return borrowing;
        }

        public async Task<DeviceBorrowing?> ApproveAsync(int id)
        {
            var borrowing = await GetByIdAsync(id);
            if (borrowing == null) return null;

            borrowing.IsApproved = true;
            _context.DeviceBorrowings.Update(borrowing);
            await _context.SaveChangesAsync();
            return borrowing;
        }

        public async Task<DeviceBorrowing?> ReturnAsync(int id, DeviceStatus status)
        {
            var borrowing = await GetByIdAsync(id);
            if (borrowing == null) return null;

            borrowing.DeviceStatus = status;
            _context.DeviceBorrowings.Update(borrowing);
            await _context.SaveChangesAsync();
            return borrowing;
        }
    }
}