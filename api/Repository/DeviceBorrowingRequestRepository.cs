using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Repositories
{
    public class DeviceBorrowingRequestRepository : IDeviceBorrowingRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceBorrowingRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateRequestAsync(DeviceBorrowingRequest request)
        {
            _context.DeviceBorrowingRequests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<DeviceBorrowingRequest?> GetRequestByIdAsync(int requestId)
        {
            return await _context.DeviceBorrowingRequests
                .Include(r => r.Device)
                .Include(r => r.FullName)
                .FirstOrDefaultAsync(r => r.Id == requestId);
        }

        public async Task UpdateRequestStatusAsync(int requestId, string status)
        {
            var request = await GetRequestByIdAsync(requestId);
            if (request != null)
            {
                request.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DeviceBorrowingRequest>> GetAllRequestsAsync()
        {
            return await _context.DeviceBorrowingRequests
                .Include(r => r.Device)
                .Include(r => r.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<DeviceBorrowingRequest>> GetBorrowingHistoryAsync(string UserName)
        {
            return await _context.DeviceBorrowingRequests
                .Where(r => r.UserName == UserName)
                .ToListAsync();
        }
    }
}
