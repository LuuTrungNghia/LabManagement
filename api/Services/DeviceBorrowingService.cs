// using api.Dtos;
// using api.Dtos.DeviceBorrowingRequest;
// using api.Interfaces;
// using api.Models;
// using AutoMapper;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace api.Services
// {
//     public class DeviceBorrowingService : IDeviceBorrowingService
//     {
//         private readonly IDeviceRepository _deviceRepository;
//         private readonly IDeviceBorrowingRequestRepository _repository;
//         private readonly IMapper _mapper;

//         public DeviceBorrowingService(IDeviceRepository deviceRepository, IDeviceBorrowingRequestRepository repository, IMapper mapper)
//         {
//             _deviceRepository = deviceRepository;
//             _repository = repository;
//             _mapper = mapper;
//         }

//         public async Task<ServiceResultDto<DeviceBorrowingRequestDto>> BorrowDeviceAsync(RequestBorrowingDeviceDto dto)
//         {
//             var requestList = new List<DeviceBorrowingRequest>();
//             var notAvailableDevices = new List<int>();
            
//             foreach (var deviceId in dto.DeviceIds)
//             {
//                 var device = await _deviceRepository.GetByIdAsync(deviceId);
//                 if (device == null || device.DeviceItems.Count <= 0)
//                 {
//                     notAvailableDevices.Add(deviceId);
//                     continue;
//                 }

//                 var request = new DeviceBorrowingRequest
//                 {
//                     DeviceId = deviceId,
//                     UserName = dto.UserName,
//                     BorrowDate = dto.BorrowDate,
//                     Status = "Pending"
//                 };
                
//                 await _repository.CreateRequestAsync(request);
//                 requestList.Add(request);

//                 var borrowedDeviceItem = device.DeviceItems.FirstOrDefault();
//                 if (borrowedDeviceItem != null)
//                 {
//                     device.DeviceItems.Remove(borrowedDeviceItem);
//                 }
                
//                 var updateDeviceStatus = device.DeviceItems.Count == 0 ? "Borrowed" : "Partially Borrowed";
//                 await _deviceRepository.UpdateDeviceStatusAsync(deviceId, updateDeviceStatus);
//             }
            
//             if (notAvailableDevices.Any())
//                 return ServiceResultDto<DeviceBorrowingRequestDto>.Failure("Some devices are unavailable.", null);
            
//             var resultDto = _mapper.Map<DeviceBorrowingRequestDto>(requestList);
//             return ServiceResultDto<DeviceBorrowingRequestDto>.Success(resultDto);
//         }

//         public async Task<ServiceResultDto<DeviceBorrowingRequestDto>> UpdateRequestStatusAsync(UpdateRequestStatusDto dto)
//         {
//             var request = await _repository.GetRequestByIdAsync(dto.RequestId);
//             if (request == null)
//                 return ServiceResultDto<DeviceBorrowingRequestDto>.Failure("Request not found", null);

//             await _repository.UpdateRequestStatusAsync(dto.RequestId, dto.Status);
//             var resultDto = _mapper.Map<DeviceBorrowingRequestDto>(request);
//             return ServiceResultDto<DeviceBorrowingRequestDto>.Success(resultDto);
//         }

//         public async Task<ServiceResultDto<DeviceBorrowingRequestDto>> GetRequestByIdAsync(int requestId)
//         {
//             var request = await _repository.GetRequestByIdAsync(requestId);
//             if (request == null)
//                 return ServiceResultDto<DeviceBorrowingRequestDto>.Failure("Request not found", null);

//             var resultDto = _mapper.Map<DeviceBorrowingRequestDto>(request);
//             return ServiceResultDto<DeviceBorrowingRequestDto>.Success(resultDto);
//         }

//         public async Task<ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>> GetAllRequestsAsync()
//         {
//             var requests = await _repository.GetAllRequestsAsync();
//             var resultDto = _mapper.Map<IEnumerable<DeviceBorrowingRequestDto>>(requests);
//             return ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>.Success(resultDto);
//         }

//         public async Task<ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>> GetBorrowingHistoryAsync(string userName)
//         {
//             var requests = await _repository.GetBorrowingHistoryAsync(userName);
//             var resultDto = _mapper.Map<IEnumerable<DeviceBorrowingRequestDto>>(requests);
//             return ServiceResultDto<IEnumerable<DeviceBorrowingRequestDto>>.Success(resultDto);
//         }
//     }
// }
