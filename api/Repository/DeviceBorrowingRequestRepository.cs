// using System.Collections.Generic;
// using System.Threading.Tasks;
// using api.Data;
// using api.Interfaces;
// using api.Models;
// using Microsoft.EntityFrameworkCore;

// namespace api.Repositories
// {
//     public class DeviceBorrowingRequestRepository : IDeviceBorrowingRequestRepository
//     {
//         private readonly ApplicationDbContext _context;

//         public DeviceBorrowingRequestRepository(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         public async Task<DeviceBorrowingRequest> GetRequestByIdAsync(int id)
//         {
//             return await _context.DeviceBorrowingRequests
//                                  .Include(r => r.DeviceItems)
//                                  .FirstOrDefaultAsync(r => r.Id == id);
//         }

//         public async Task<IEnumerable<DeviceBorrowingRequest>> GetAllRequestsAsync()
//         {
//             return await _context.DeviceBorrowingRequests.Include(r => r.DeviceItems).ToListAsync();
//         }

//         public async Task AddRequestAsync(DeviceBorrowingRequest request)
//         {
//             _context.DeviceBorrowingRequests.Add(request);
//             await _context.SaveChangesAsync();
//         }

//         public async Task UpdateRequestAsync(DeviceBorrowingRequest request)
//         {
//             _context.DeviceBorrowingRequests.Update(request);
//             await _context.SaveChangesAsync();
//         }

//         public async Task DeleteRequestAsync(int id)
//         {
//             var request = await _context.DeviceBorrowingRequests.FindAsync(id);
//             if (request != null)
//             {
//                 _context.DeviceBorrowingRequests.Remove(request);
//                 await _context.SaveChangesAsync();
//             }
//         }
//     }
// }
