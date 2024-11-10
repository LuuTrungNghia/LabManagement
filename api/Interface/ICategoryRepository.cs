using api.Models;

namespace api.Interfaces
{
    public interface ICategoryRepository
    {
        Task CreateAsync(Category category);
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
    }
}
