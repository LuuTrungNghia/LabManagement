using api.Dtos;
using api.Dtos.DeviceBorrowing;
using api.Interfaces;
using api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Services
{
    public class DeviceBorrowingService : IDeviceBorrowingService
    {
        private readonly IDeviceBorrowingRequestRepository _repository;

        public DeviceBorrowingService(IDeviceBorrowingRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResultDto<DeviceBorrowingRequestDto>> BorrowDeviceAsync(RequestBorrowingDeviceDto dto)
        {
            var request = new DeviceBorrowingRequest
            {
                DeviceId = dto.DeviceId,
                UserId = dto.UserId,
                BorrowDate = dto.BorrowDate,
                Status = "Pending"
            };

            await _repository.CreateRequestAsync(request);

            return new ServiceResultDto<DeviceBorrowingRequestDto>
            {
                Success = true,
                Data = new DeviceBorrowingRequestDto
                {
                    Id = request.Id,
                    DeviceId = request.DeviceId,
                    UserId = request.UserId,
                    BorrowDate = request.BorrowDate,
                    Status = request.Status
                }
            };
        }

        public async Task<ServiceResultDto<DeviceBorrowingRequestDto>> UpdateRequestStatusAsync(UpdateRequestStatusDto dto)
        {
            await _repository.UpdateRequestStatusAsync(dto.RequestId, dto.Status);
            return new ServiceResultDto<DeviceBorrowingRequestDto> { Success = true };
        }

        public async Task<ServiceResultDto<DeviceBorrowingRequestDto>> GetRequestByIdAsync(int requestId)
        {
            var request = await _repository.GetRequestByIdAsync(requestId);
            return request == null ? new ServiceResultDto<DeviceBorrowingRequestDto> { Success = false, Message = "Request not found" } : new ServiceResultDto<DeviceBorrowingRequestDto>
            {
                Success = true,
                Data = new DeviceBorrowingRequestDto
                {
                    Id = request.Id,
                    DeviceId = request.DeviceId,
                    UserId = request.UserId,
                    BorrowDate = request.BorrowDate,
                    Status = request.Status
                }
            };
        }

        public async Task<ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>> GetAllRequestsAsync()
        {
            var requests = await _repository.GetAllRequestsAsync();
            var requestDtos = requests.Select(request => new DeviceBorrowingRequestDto
            {
                Id = request.Id,
                DeviceId = request.DeviceId,
                UserId = request.UserId,
                BorrowDate = request.BorrowDate,
                Status = request.Status
            }).ToList();

            return new ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>
            {
                Success = true,
                Data = requestDtos
            };
        }

        public async Task<ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>> GetBorrowingHistoryAsync(string userId)
        {
            var history = await _repository.GetBorrowingHistoryAsync(userId);
            var historyDtos = history.Select(request => new DeviceBorrowingRequestDto
            {
                Id = request.Id,
                DeviceId = request.DeviceId,
                UserId = request.UserId,
                BorrowDate = request.BorrowDate,
                Status = request.Status
            }).ToList();

            return new ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>
            {
                Success = true,
                Data = historyDtos
            };
        }
    }
}
