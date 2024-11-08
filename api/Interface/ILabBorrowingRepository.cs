using api.Models;

namespace api.Interfaces
{
    public interface ILabBorrowingRepository
    {
        Task<LabBorrowing?> CreateAsync(LabBorrowing borrowing);
        Task<IEnumerable<LabBorrowing>> GetAllByUserAsync(string userName);
        Task<LabBorrowing?> ApproveAsync(int id);
    }
}
