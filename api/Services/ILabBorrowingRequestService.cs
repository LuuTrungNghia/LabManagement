using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dtos;

namespace api.Services
{
    public interface ILabBorrowingRequestService
    {
        Task<LabBorrowingRequestDto> CreateLabBorrowingRequestAsync(CreateLabBorrowingRequestDto dto);
        Task<LabBorrowingRequestDto> GetLabBorrowingRequestByIdAsync(int id);
        Task<IEnumerable<LabBorrowingRequestDto>> GetAllLabBorrowingRequestsAsync();
        Task<LabBorrowingRequestDto> UpdateLabBorrowingRequestAsync(int id, UpdateLabBorrowingRequestDto dto);
        Task<bool> DeleteLabBorrowingRequestAsync(int id);
        Task<LabBorrowingRequestDto> ApproveLabBorrowingRequestAsync(int id);
        Task<LabBorrowingRequestDto> RejectLabBorrowingRequestAsync(int id);
    }
}
