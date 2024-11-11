// using api.Dtos.Device;
// using api.Dtos.DeviceBorrowing;
// using api.Interfaces;
// using api.Mappers;
// using api.Models;
// using Microsoft.AspNetCore.Identity;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace api.Services
// {
//     public class DeviceBorrowingService
//     {
//         private readonly IDeviceBorrowingRequestRepository _repository;
//         private readonly IDeviceRepository _deviceRepository;
//         private readonly UserManager<ApplicationUser> _userManager;

//         public DeviceBorrowingService(IDeviceBorrowingRequestRepository repository, IDeviceRepository deviceRepository, UserManager<ApplicationUser> userManager)
//         {
//             _repository = repository;
//             _deviceRepository = deviceRepository;
//             _userManager = userManager;
//         }

//         public async Task CreateRequestAsync(CreateDeviceBorrowingRequestDto dto)
//         {
//             var request = new DeviceBorrowingRequest
//             {
//                 DeviceId = dto.DeviceId,
//                 RequestedQuantity = dto.RequestedQuantity,
//                 FromDate = dto.FromDate,
//                 ToDate = dto.ToDate,
//                 Status = DeviceItemStatus.Available
//             };

//             await _repository.CreateAsync(request);
//         }

//         public async Task ApproveRequestAsync(int requestId, string approvedById)
//         {
//             var request = await _repository.GetByIdAsync(requestId);
//             if (request == null) return;

//             var approvedUser = await _userManager.FindByIdAsync(approvedById);
//             if (approvedUser == null) return;

//             request.Status = DeviceItemStatus.Borrowed;
//             request.ApprovedDate = DateTime.Now;
//             request.ApprovedById = approvedById;

//             await _repository.UpdateAsync(request);
//         }

//         public async Task ConfirmReturnAsync(int requestId)
//         {
//             var request = await _repository.GetByIdAsync(requestId);
//             if (request == null) return;

//             request.IsReturned = true;
//             request.ReturnDate = DateTime.Now;
//             request.Status = DeviceItemStatus.Available;

//             await _repository.UpdateAsync(request);
//         }

//         public async Task<IEnumerable<DeviceBorrowingRequestDto>> GetHistoryAsync()
//         {
//             var requests = await _repository.GetAllAsync();
//             return requests.Select(r => r.ToDto()).ToList();
//         }
//     }
// }
