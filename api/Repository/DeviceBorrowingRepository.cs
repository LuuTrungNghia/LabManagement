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
            // Include device details for each borrowing request.
            return await _context.DeviceBorrowingRequests
                .Include(r => r.GroupStudents)  // Nếu cần lấy thông tin sinh viên nhóm
                .Include(r => r.DeviceBorrowingDetails)  // Nếu cần lấy thông tin chi tiết thiết bị mượn
                .ToListAsync();
        }

        public async Task<DeviceBorrowingRequest> GetByIdAsync(int id)
        {
            return await _context.DeviceBorrowingRequests
                .Include(r => r.DeviceBorrowingDetails) // Bao gồm chi tiết thiết bị
                .ThenInclude(d => d.DeviceItem)
                .FirstOrDefaultAsync(r => r.Id == id); // Lấy yêu cầu theo ID
        }

        public async Task UpdateAsync(DeviceBorrowingRequest deviceBorrowingRequest)
        {
            _context.DeviceBorrowingRequests.Update(deviceBorrowingRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DeviceBorrowingRequest>> GetDeviceBorrowingHistory(string username)
        {
            return await _context.DeviceBorrowingRequests
                .Where(r => r.Username == username) // Lọc theo username
                .Include(r => r.GroupStudents)
                .Include(r => r.DeviceBorrowingDetails)
                .ToListAsync();
        }

        public async Task<bool> MarkDeviceAsReturned(int deviceId, int deviceItemId)
        {
            var deviceItem = await _context.DeviceItems.FirstOrDefaultAsync(d => d.DeviceId == deviceId && d.DeviceItemId == deviceItemId);
            if (deviceItem == null)
                return false;

            deviceItem.DeviceItemStatus = DeviceItemStatus.Borrowed;
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
                .Include(r => r.DeviceBorrowingDetails)
                .FirstOrDefaultAsync(r => r.DeviceBorrowingDetails.Any(d => d.DeviceItemId == deviceItemId && 
                                                                            r.Status != DeviceBorrowingStatus.Completed));
        }
        public async Task DeleteAsync(DeviceBorrowingRequest request)
        {
            _context.DeviceBorrowingRequests.Remove(request);
            await _context.SaveChangesAsync();
        }

        public async Task<DeviceBorrowingRequest> CheckDeviceAvailability(int deviceItemId, DateTime startDate, DateTime endDate)
        {
            return await _context.DeviceBorrowingRequests
                .Include(r => r.DeviceBorrowingDetails)
                .FirstOrDefaultAsync(r => r.DeviceBorrowingDetails
                    .Any(d => d.DeviceItemId == deviceItemId &&
                            r.Status != DeviceBorrowingStatus.Completed && 
                            ((d.StartDate < endDate && d.EndDate > startDate) || // Kiểm tra thời gian trùng
                            (d.StartDate >= startDate && d.StartDate < endDate) || 
                            (d.EndDate > startDate && d.EndDate <= endDate)
                            )));
        }
    }
}
