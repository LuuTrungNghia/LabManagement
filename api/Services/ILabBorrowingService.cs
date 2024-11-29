using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dtos;

namespace api.Services
{
    public interface ILabBorrowingService
    {
        Task<LabBorrowingRequestDto> CreateLabBorrowingRequestAsync(CreateLabBorrowingRequestDto dto);
        Task<LabBorrowingRequestDto> GetLabBorrowingRequestByIdAsync(int id);
        Task<IEnumerable<LabBorrowingRequestDto>> GetAllLabBorrowingRequestsAsync();
        Task<LabBorrowingRequestDto> UpdateLabBorrowingRequestAsync(int id, UpdateLabBorrowingRequestDto dto);
        Task<bool> DeleteLabBorrowingRequestAsync(int id);
        Task<bool> ApproveLabBorrowingRequestAsync(int id);
        Task<bool> RejectLabBorrowingRequestAsync(int id);
        Task<IEnumerable<LabBorrowingRequestHistoryDto>> GetLabBorrowingHistoryAsync(string username);
    }
}
