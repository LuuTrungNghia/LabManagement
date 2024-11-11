// using api.Data;
// using api.Interfaces;
// using api.Models;
// using Microsoft.EntityFrameworkCore;

// namespace api.Repositories
// {
//     public class LabRepository : ILabRepository
//     {
//         private readonly ApplicationDbContext _context;

//         public LabRepository(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         public async Task<IEnumerable<Lab>> GetAllAsync() => await _context.Labs.ToListAsync();

//         public async Task<Lab?> GetByIdAsync(int id) => await _context.Labs.FindAsync(id);

//         public async Task CreateAsync(Lab lab)
//         {
//             await _context.Labs.AddAsync(lab);
//             await _context.SaveChangesAsync();
//         }

//         public async Task<Lab?> UpdateAsync(Lab lab)
//         {
//             _context.Labs.Update(lab);
//             await _context.SaveChangesAsync();

//             return lab;
//         }

//         public async Task<Lab?> DeleteAsync(int id)
//         {
//             var lab = await GetByIdAsync(id);
//             if (lab == null) return null;

//             _context.Labs.Remove(lab);
//             await _context.SaveChangesAsync();

//             return lab;
//         }
//     }
// }
