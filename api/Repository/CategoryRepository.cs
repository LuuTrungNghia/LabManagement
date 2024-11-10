using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> GetByIdAsync(int id) => await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

        public async Task<IEnumerable<Category>> GetAllAsync() => await _context.Categories.ToListAsync();
    }
}
