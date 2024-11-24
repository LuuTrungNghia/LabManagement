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
            if (request == null) return null;

            _context.LabBorrowingRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<LabBorrowingRequest> GetLabBorrowingRequestByIdAsync(int id)
        {
            if (id <= 0) return null;

            return await _context.LabBorrowingRequests.FindAsync(id);
        }

        public async Task<IEnumerable<LabBorrowingRequest>> GetAllLabBorrowingRequestsAsync()
        {
            return await _context.LabBorrowingRequests.ToListAsync();
        }

        public async Task<LabBorrowingRequest> UpdateLabBorrowingRequestAsync(LabBorrowingRequest request)
        {
            var existingRequest = await _context.LabBorrowingRequests.FindAsync(request.Id);
            if (existingRequest == null)
            {
                return null; // or handle as needed
            }
            _context.LabBorrowingRequests.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<bool> DeleteLabBorrowingRequestAsync(int id)
        {
            if (id <= 0) return false;

            var request = await _context.LabBorrowingRequests.FindAsync(id);
            if (request == null) return false;

            _context.LabBorrowingRequests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
