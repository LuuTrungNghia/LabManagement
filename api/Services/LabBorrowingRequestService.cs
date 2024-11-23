// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using api.Data;
// using api.Models;
// using Microsoft.EntityFrameworkCore;

// namespace api.Services
// {
//     public class LabBorrowingRequestService : ILabBorrowingRequestService
//     {
//         private readonly ApplicationDbContext _context;

//         public LabBorrowingRequestService(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         // Tạo mới yêu cầu mượn phòng lab
//         public async Task<LabBorrowingRequest> CreateLabBorrowingRequestAsync(LabBorrowingRequest labBorrowingRequest)
//         {
//             var user = await _context.Users.FindAsync(labBorrowingRequest.UserId);
//             var lab = await _context.Labs.FindAsync(labBorrowingRequest.LabId);

//             if (user == null || lab == null)
//             {
//                 return null; // Không tìm thấy người dùng hoặc phòng lab
//             }

//             _context.LabBorrowingRequests.Add(labBorrowingRequest);
//             await _context.SaveChangesAsync();

//             return labBorrowingRequest;
//         }

//         // Phê duyệt yêu cầu mượn phòng lab
//         public async Task<LabBorrowingRequest> ApproveLabBorrowingRequestAsync(int requestId)
//         {
//             var request = await _context.LabBorrowingRequests.FindAsync(requestId);
//             if (request == null) return null;

//             request.IsApproved = true;
//             await _context.SaveChangesAsync();

//             return request;
//         }

//         // Lấy lịch sử mượn phòng của người dùng
//         public async Task<IEnumerable<LabBorrowingRequest>> GetLabBorrowingHistoryAsync(int userId)
//         {
//             return await _context.LabBorrowingRequests
//                 .Where(r => r.UserId == userId)
//                 .ToListAsync();
//         }

//         // Lấy thông tin một yêu cầu mượn phòng cụ thể
//         public async Task<LabBorrowingRequest> GetLabBorrowingRequestAsync(int requestId)
//         {
//             return await _context.LabBorrowingRequests
//                 .Include(r => r.DeviceBorrowingRequests)
//                 .ThenInclude(d => d.DeviceBorrowingDetails)
//                 .FirstOrDefaultAsync(r => r.LabBorrowingRequestId == requestId);
//         }
//     }
// }
