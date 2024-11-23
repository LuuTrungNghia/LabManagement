// using api.Data;
// using api.Models;
// using Microsoft.EntityFrameworkCore;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace api.Repositories
// {
//     public class LabRepository : ILabRepository
//     {
//         private readonly ApplicationDbContext _context;

//         public LabRepository(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         public async Task<IEnumerable<Lab>> GetAllLabsAsync()
//         {
//             return await _context.Labs.ToListAsync();
//         }

//         public async Task<Lab> GetLabByIdAsync(int id)
//         {
//             return await _context.Labs.FindAsync(id);
//         }

//         public async Task AddLabAsync(Lab lab)
//         {
//             await _context.Labs.AddAsync(lab);
//             await _context.SaveChangesAsync();
//         }

//         public async Task UpdateLabAsync(Lab lab)
//         {
//             _context.Labs.Update(lab);
//             await _context.SaveChangesAsync();
//         }

//         public async Task DeleteLabAsync(Lab lab)
//         {
//             _context.Labs.Remove(lab);
//             await _context.SaveChangesAsync();
//         }
//     }
// }
