using api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IDeviceBorrowingRequestRepository
    {
        Task CreateRequestAsync(DeviceBorrowingRequest request);
        Task<DeviceBorrowingRequest?> GetRequestByIdAsync(int requestId);
        Task UpdateRequestStatusAsync(int requestId, string status);
        Task<IEnumerable<DeviceBorrowingRequest>> GetAllRequestsAsync();
        Task<IEnumerable<DeviceBorrowingRequest>> GetBorrowingHistoryAsync(string userName);
    }
}
