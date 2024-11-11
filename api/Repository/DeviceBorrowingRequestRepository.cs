// using api.Data;
// using api.Interfaces;
// using api.Models;
// using Microsoft.EntityFrameworkCore;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace api.Repositories
// {
//     public class DeviceBorrowingRequestRepository : IDeviceBorrowingRequestRepository
//     {
//         private readonly ApplicationDbContext _context;

//         public DeviceBorrowingRequestRepository(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         public async Task<IEnumerable<DeviceBorrowingRequest>> GetAllAsync()
//         {
//             return await _context.DeviceBorrowingRequests.Include(d => d.Device).ToListAsync();
//         }

//         public async Task<DeviceBorrowingRequest?> GetByIdAsync(int requestId)
//         {
//             return await _context.DeviceBorrowingRequests.Include(d => d.Device)
//                                                          .FirstOrDefaultAsync(r => r.DeviceBorrowingRequestId == requestId);
//         }

//         public async Task CreateAsync(DeviceBorrowingRequest request)
//         {
//             _context.DeviceBorrowingRequests.Add(request);
//             await _context.SaveChangesAsync();
//         }

//         public async Task UpdateAsync(DeviceBorrowingRequest request)
//         {
//             _context.DeviceBorrowingRequests.Update(request);
//             await _context.SaveChangesAsync();
//         }
//     }
// }
