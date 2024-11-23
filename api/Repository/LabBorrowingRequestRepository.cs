// using api.Data;
// using api.Models;
// using Microsoft.EntityFrameworkCore;

// public class LabBorrowingRequestRepository : ILabBorrowingRequestRepository
// {
//     private readonly ApplicationDbContext _context;

//     public LabBorrowingRequestRepository(ApplicationDbContext context)
//     {
//         _context = context;
//     }

//     public async Task<IEnumerable<LabBorrowingRequest>> GetAllRequestsAsync()
//     {
//         return await _context.LabBorrowingRequests
//             .Include(r => r.Lab)
//             .Include(r => r.User)
//             .Include(r => r.LabBorrowingDetails)
//             .ThenInclude(d => d.Device)
//             .ToListAsync();
//     }

//     public async Task<LabBorrowingRequest> GetRequestByIdAsync(int requestId)
//     {
//         return await _context.LabBorrowingRequests
//             .Include(r => r.Lab)
//             .Include(r => r.User)
//             .Include(r => r.LabBorrowingDetails)
//             .ThenInclude(d => d.Device)
//             .FirstOrDefaultAsync(r => r.LabBorrowingRequestId == requestId);
//     }

//     public async Task<LabBorrowingRequest> CreateRequestAsync(LabBorrowingRequest request)
//     {
//         _context.LabBorrowingRequests.Add(request);
//         await _context.SaveChangesAsync();
//         return request;
//     }

//     public async Task<LabBorrowingRequest> ApproveRequestAsync(int requestId)
//     {
//         var request = await _context.LabBorrowingRequests.FindAsync(requestId);
//         if (request == null) return null;
//         request.IsApproved = true;
//         _context.LabBorrowingRequests.Update(request);
//         await _context.SaveChangesAsync();
//         return request;
//     }

//     public async Task<IEnumerable<LabBorrowingRequest>> GetRequestHistoryAsync(string userId)
//     {
//         return await _context.LabBorrowingRequests
//             .Where(r => r.UserId == userId)
//             .Include(r => r.Lab)
//             .Include(r => r.LabBorrowingDetails)
//             .ThenInclude(d => d.Device)
//             .ToListAsync();
//     }
// }
