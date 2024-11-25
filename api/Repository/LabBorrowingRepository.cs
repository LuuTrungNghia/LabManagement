using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;
using api.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class LabBorrowingRepository : ILabBorrowingRepository
    {
        private readonly ApplicationDbContext _context;

        public LabBorrowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LabBorrowingRequest> CreateLabBorrowingRequestAsync(LabBorrowingRequest request)
        {
            _context.LabBorrowingRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<LabBorrowingRequest> GetLabBorrowingRequestByIdAsync(int id)
        {
            return await _context.LabBorrowingRequests
                .Include(r => r.GroupStudents)
                .Include(r => r.DeviceBorrowingDetails)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<LabBorrowingRequest>> GetAllLabBorrowingRequestsAsync()
        {
            return await _context.LabBorrowingRequests
                .Include(r => r.GroupStudents)
                .Include(r => r.DeviceBorrowingDetails)
                .ToListAsync();
        }

        public async Task<LabBorrowingRequest> UpdateLabBorrowingRequestAsync(LabBorrowingRequest request)
        {
            _context.LabBorrowingRequests.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<bool> DeleteLabBorrowingRequestAsync(int id)
        {
            var request = await _context.LabBorrowingRequests.FindAsync(id);
            if (request == null) return false;

            _context.LabBorrowingRequests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
