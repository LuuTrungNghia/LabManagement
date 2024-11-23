// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using api.Models;
// using api.Data;

// namespace api.Repositories
// {
//     public class LabBorrowingRequestRepository : ILabBorrowingRepository
//     {
//         private readonly ApplicationDbContext _context;

//         public LabBorrowingRequestRepository(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         // Tạo mới yêu cầu mượn phòng lab
//         public async Task<LabBorrowingRequest> CreateLabBorrowingRequestAsync(LabBorrowingRequest labBorrowingRequest)
//         {
//             _context.LabBorrowingRequests.Add(labBorrowingRequest);
//             await _context.SaveChangesAsync();
//             return labBorrowingRequest;
//         }

//         // Lấy thông tin yêu cầu mượn phòng lab theo ID
//         public async Task<LabBorrowingRequest> GetLabBorrowingRequestByIdAsync(int id)
//         {
//             return await _context.LabBorrowingRequests
//                 .Include(r => r.DeviceBorrowingRequests)
//                 .ThenInclude(d => d.DeviceBorrowingDetails)
//                 .FirstOrDefaultAsync(r => r.LabBorrowingRequestId == id);
//         }

//         // Lấy tất cả yêu cầu mượn phòng lab
//         public async Task<IEnumerable<LabBorrowingRequest>> GetAllLabBorrowingRequestsAsync()
//         {
//             return await _context.LabBorrowingRequests
//                 .Include(r => r.DeviceBorrowingRequests)
//                 .ThenInclude(d => d.DeviceBorrowingDetails)
//                 .ToListAsync();
//         }

//         // Phê duyệt yêu cầu mượn phòng lab
//         public async Task<LabBorrowingRequest> ApproveLabBorrowingRequestAsync(int id)
//         {
//             var request = await _context.LabBorrowingRequests.FindAsync(id);
//             if (request != null)
//             {
//                 request.IsApproved = true;
//                 await _context.SaveChangesAsync();
//             }
//             return request;
//         }
//     }
// }
