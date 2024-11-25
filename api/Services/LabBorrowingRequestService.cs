using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dtos;
using api.Repositories;
using api.Mappers;

namespace api.Services
{
    public class LabBorrowingRequestService : ILabBorrowingRequestService
    {
        private readonly ILabBorrowingRepository _repository;

        public LabBorrowingRequestService(ILabBorrowingRepository repository)
        {
            _repository = repository;
        }

        public async Task<LabBorrowingRequestDto> CreateLabBorrowingRequestAsync(CreateLabBorrowingRequestDto dto)
        {
            var request = LabBorrowingMapper.ToModel(dto);
            var createdRequest = await _repository.CreateLabBorrowingRequestAsync(request);
            return LabBorrowingMapper.ToDto(createdRequest);
        }

        public async Task<LabBorrowingRequestDto> GetLabBorrowingRequestByIdAsync(int id)
        {
            var request = await _repository.GetLabBorrowingRequestByIdAsync(id);
            return request == null ? null : LabBorrowingMapper.ToDto(request);
        }

        public async Task<IEnumerable<LabBorrowingRequestDto>> GetAllLabBorrowingRequestsAsync()
        {
            var requests = await _repository.GetAllLabBorrowingRequestsAsync();
            return requests.Select(LabBorrowingMapper.ToDto);
        }

        public async Task<LabBorrowingRequestDto> UpdateLabBorrowingRequestAsync(int id, UpdateLabBorrowingRequestDto dto)
        {
            var existingRequest = await _repository.GetLabBorrowingRequestByIdAsync(id);
            if (existingRequest == null) return null;

            var updatedRequest = LabBorrowingMapper.ToModel(dto);
            updatedRequest.Id = id;
            var result = await _repository.UpdateLabBorrowingRequestAsync(updatedRequest);
            return LabBorrowingMapper.ToDto(result);
        }

        public async Task<bool> DeleteLabBorrowingRequestAsync(int id)
        {
            return await _repository.DeleteLabBorrowingRequestAsync(id);
        }

        // Approve the lab borrowing request
        public async Task<LabBorrowingRequestDto> ApproveLabBorrowingRequestAsync(int id)
        {
            var request = await _repository.GetLabBorrowingRequestByIdAsync(id);
            if (request == null || request.Status != LabBorrowingStatus.Pending)
                return null; // Only approve pending requests

            request.Status = LabBorrowingStatus.Approved;
            var updatedRequest = await _repository.UpdateLabBorrowingRequestAsync(request);
            return LabBorrowingMapper.ToDto(updatedRequest);
        }

        // Reject the lab borrowing request
        public async Task<LabBorrowingRequestDto> RejectLabBorrowingRequestAsync(int id)
        {
            var request = await _repository.GetLabBorrowingRequestByIdAsync(id);
            if (request == null || request.Status != LabBorrowingStatus.Pending)
                return null; // Only reject pending requests

            request.Status = LabBorrowingStatus.Rejected;
            var updatedRequest = await _repository.UpdateLabBorrowingRequestAsync(request);
            return LabBorrowingMapper.ToDto(updatedRequest);
        }
    }
}
