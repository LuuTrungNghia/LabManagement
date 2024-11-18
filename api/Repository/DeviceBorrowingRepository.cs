using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Repositories
{
    public class DeviceBorrowingRepository : IDeviceBorrowingRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceBorrowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DeviceBorrowingRequest deviceBorrowingRequest)
        {
            await _context.DeviceBorrowingRequests.AddAsync(deviceBorrowingRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DeviceBorrowingRequest>> GetAllAsync()
        {
            return await _context.DeviceBorrowingRequests.Include(d => d.DeviceBorrowingDetails).ToListAsync();
        }

        public async Task<DeviceBorrowingRequest> GetByIdAsync(int id)
        {
            return await _context.DeviceBorrowingRequests.Include(d => d.DeviceBorrowingDetails)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(DeviceBorrowingRequest deviceBorrowingRequest)
        {
            _context.DeviceBorrowingRequests.Update(deviceBorrowingRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DeviceBorrowingRequest>> GetDeviceBorrowingHistory(string username)
        {
            return await _context.DeviceBorrowingRequests.Include(d => d.DeviceBorrowingDetails)
                .Where(r => r.Username == username)
                .ToListAsync();
        }

        public async Task<bool> MarkDeviceAsReturned(int deviceId, int deviceItemId)
        {
            var deviceItem = await _context.DeviceItems.FirstOrDefaultAsync(d => d.DeviceId == deviceId && d.DeviceItemId == deviceItemId);
            if (deviceItem == null)
                return false;

            deviceItem.DeviceItemStatus = DeviceItemStatus.Available;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DeviceBorrowingRequest>> GetByUsernameAsync(string username)
        {
            return await _context.DeviceBorrowingRequests
                .Where(r => r.Username == username)
                .Include(d => d.DeviceBorrowingDetails)
                .ToListAsync();
        }

        public async Task<DeviceBorrowingRequest> GetByDeviceItemIdAsync(int deviceItemId)
        {
            return await _context.DeviceBorrowingRequests
                .FirstOrDefaultAsync(r => r.DeviceBorrowingDetails
                .Any(d => d.DeviceItemId == deviceItemId));
        }
    }
}