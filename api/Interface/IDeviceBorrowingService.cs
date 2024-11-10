using api.Dtos;
using api.Dtos.DeviceBorrowingRequest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IDeviceBorrowingService
    {
        Task<ServiceResultDto<DeviceBorrowingRequestDto>> BorrowDeviceAsync(RequestBorrowingDeviceDto dto);
        Task<ServiceResultDto<DeviceBorrowingRequestDto>> UpdateRequestStatusAsync(UpdateRequestStatusDto dto);
        Task<ServiceResultDto<DeviceBorrowingRequestDto>> GetRequestByIdAsync(int requestId);
        Task<ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>> GetAllRequestsAsync();
        Task<ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>> GetBorrowingHistoryAsync(string userName);
    }
}
