// using System.Collections.Generic;
// using System.Threading.Tasks;
// using api.Interfaces;
// using api.Models;

// namespace api.Services
// {
//     public class DeviceBorrowingService : IDeviceBorrowingService
//     {
//         private readonly IDeviceBorrowingRequestRepository _repository;

//         public DeviceBorrowingService(IDeviceBorrowingRequestRepository repository)
//         {
//             _repository = repository;
//         }

//         public async Task<DeviceBorrowingRequest> GetRequestByIdAsync(int id) => await _repository.GetRequestByIdAsync(id);

//         public async Task<IEnumerable<DeviceBorrowingRequest>> GetAllRequestsAsync() => await _repository.GetAllRequestsAsync();

//         public async Task AddRequestAsync(DeviceBorrowingRequest request) => await _repository.AddRequestAsync(request);

//         public async Task UpdateRequestAsync(DeviceBorrowingRequest request) => await _repository.UpdateRequestAsync(request);

//         public async Task DeleteRequestAsync(int id) => await _repository.DeleteRequestAsync(id);
//     }
// }
