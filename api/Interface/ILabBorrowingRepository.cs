using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Repositories
{
    public interface ILabBorrowingRepository
    {
        Task<LabBorrowingRequest> CreateLabBorrowingRequestAsync(LabBorrowingRequest request);
        Task<LabBorrowingRequest> GetLabBorrowingRequestByIdAsync(int id);
        Task<IEnumerable<LabBorrowingRequest>> GetAllLabBorrowingRequestsAsync();
        Task<LabBorrowingRequest> UpdateLabBorrowingRequestAsync(LabBorrowingRequest request);
        Task<bool> DeleteLabBorrowingRequestAsync(int id);
    }
}
