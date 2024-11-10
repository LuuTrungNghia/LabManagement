using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Device;
using api.Dtos.DeviceBorrowingRequest;
using api.Interfaces;
using api.Models;
using AutoMapper;

namespace api.Services
{
    public class DeviceBorrowingService : IDeviceBorrowingService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceBorrowingRequestRepository _repository;
        private readonly IMapper _mapper;

        public DeviceBorrowingService(IDeviceRepository deviceRepository, IDeviceBorrowingRequestRepository repository, IMapper mapper)
        {
            _deviceRepository = deviceRepository;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResultDto<DeviceBorrowingRequestDto>> BorrowDeviceAsync(RequestBorrowingDeviceDto dto)
        {
            var requestList = new List<DeviceBorrowingRequest>();
            var notAvailableDevices = new List<int>();
            
            // Kiểm tra từng thiết bị
            foreach (var deviceId in dto.DeviceIds)
            {
                var device = await _deviceRepository.GetByIdAsync(deviceId);
                if (device == null)
                {
                    notAvailableDevices.Add(deviceId);  // Thêm vào danh sách nếu không tìm thấy thiết bị
                    continue;
                }

                if (device.DeviceItems.Count <= 0)
                {
                    notAvailableDevices.Add(deviceId);
                    continue;
                }

                var request = new DeviceBorrowingRequest
                {
                    DeviceId = deviceId,
                    UserName = dto.UserName,
                    BorrowDate = dto.BorrowDate,
                    Status = "Pending"
                };
                
                await _repository.CreateRequestAsync(request);
                requestList.Add(request);

                var borrowedDeviceItem = device.DeviceItems.FirstOrDefault();
                if (borrowedDeviceItem != null)
                {
                    device.DeviceItems.Remove(borrowedDeviceItem);
                }
                
                var updateDeviceDto = _mapper.Map<UpdateDeviceRequestDto>(device);
                await _deviceRepository.UpdateAsync(device.DeviceId, updateDeviceDto);
            }

            if (notAvailableDevices.Any())
            {
                return new ServiceResultDto<DeviceBorrowingRequestDto>
                {
                    Success = false,
                    Message = $"These devices are not available: {string.Join(", ", notAvailableDevices)}"
                };
            }

            var requestDtos = _mapper.Map<List<DeviceBorrowingRequestDto>>(requestList);
            return new ServiceResultDto<DeviceBorrowingRequestDto>
            {
                Success = true,
                Data = requestDtos.FirstOrDefault(), 
                Id = requestList.FirstOrDefault()?.Id 
            };
        }


        public async Task<ServiceResultDto<DeviceBorrowingRequestDto>> UpdateRequestStatusAsync(UpdateRequestStatusDto dto)
        {
            var request = await _repository.GetRequestByIdAsync(dto.RequestId);
            if (request == null)
            {
                return new ServiceResultDto<DeviceBorrowingRequestDto>
                {
                    Success = false,
                    Message = "Request not found"
                };
            }

            request.Status = dto.Status;
            await _repository.UpdateRequestStatusAsync(dto.RequestId, dto.Status);

            if (dto.Status == "Returned")
            {
                var device = await _deviceRepository.GetByIdAsync(request.DeviceId);
                if (device != null)
                {
                    device.DeviceItems.Add(new DeviceItem { DeviceId = device.DeviceId });
                    var updateDeviceDto = _mapper.Map<UpdateDeviceRequestDto>(device);
                    await _deviceRepository.UpdateAsync(device.DeviceId, updateDeviceDto);
                }
            }

            return new ServiceResultDto<DeviceBorrowingRequestDto>
            {
                Success = true,
                Data = _mapper.Map<DeviceBorrowingRequestDto>(request)
            };
        }

        // Implement missing methods from IDeviceBorrowingService interface
        public async Task<ServiceResultDto<DeviceBorrowingRequestDto>> GetRequestByIdAsync(int requestId)
        {
            var request = await _repository.GetRequestByIdAsync(requestId);
            if (request == null)
            {
                return new ServiceResultDto<DeviceBorrowingRequestDto>
                {
                    Success = false,
                    Message = "Request not found"
                };
            }

            var requestDto = _mapper.Map<DeviceBorrowingRequestDto>(request);
            return new ServiceResultDto<DeviceBorrowingRequestDto>
            {
                Success = true,
                Data = requestDto
            };
        }

        public async Task<ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>> GetAllRequestsAsync()
        {
            var requests = await _repository.GetAllRequestsAsync();
            var requestDtos = _mapper.Map<IEnumerable<DeviceBorrowingRequestDto>>(requests);
            return new ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>
            {
                Success = true,
                Data = requestDtos
            };
        }

        public async Task<ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>> GetBorrowingHistoryAsync(string userId)
        {
            var requests = await _repository.GetBorrowingHistoryAsync(userId);
            var requestDtos = _mapper.Map<IEnumerable<DeviceBorrowingRequestDto>>(requests);
            return new ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>
            {
                Success = true,
                Data = requestDtos
            };
        }
    }
}
