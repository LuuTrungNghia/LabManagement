using System.Collections.Generic;
using System.Linq;
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

        // Tạo mới yêu cầu mượn phòng
        public async Task<LabBorrowingRequestDto> CreateLabBorrowingRequestAsync(CreateLabBorrowingRequestDto dto)
        {
            // Kiểm tra thời gian trùng lặp cho phòng
            var existingRequests = await _repository.GetAllLabBorrowingRequestsAsync();
            foreach (var existingRequest in existingRequests)
            {
                if (IsTimeOverlapping(dto.StartDate, dto.EndDate, existingRequest.StartDate, existingRequest.EndDate))
                {
                    throw new Exception($"Conflict with existing lab booking from {existingRequest.StartDate} to {existingRequest.EndDate}");
                }
            }

            // Kiểm tra thiết bị
            foreach (var deviceDetail in dto.DeviceBorrowingDetails)
            {
                foreach (var existingRequest in existingRequests)
                {
                    foreach (var detail in existingRequest.DeviceBorrowingDetails)
                    {
                        if (IsTimeOverlapping(deviceDetail.StartDate, deviceDetail.EndDate, detail.StartDate, detail.EndDate))
                        {
                            throw new Exception($"Conflict with existing device booking from {detail.StartDate} to {detail.EndDate}");
                        }
                    }
                }
            }

            var request = LabBorrowingMapper.ToModel(dto);
            var createdRequest = await _repository.CreateLabBorrowingRequestAsync(request);
            return LabBorrowingMapper.ToDto(createdRequest);
        }

        // Cập nhật yêu cầu mượn phòng
        public async Task<LabBorrowingRequestDto> UpdateLabBorrowingRequestAsync(int id, UpdateLabBorrowingRequestDto dto)
        {
            var existingRequest = await _repository.GetLabBorrowingRequestByIdAsync(id);
            if (existingRequest == null) return null;

            var existingRequests = await _repository.GetAllLabBorrowingRequestsAsync();

            foreach (var deviceDetail in dto.DeviceBorrowingDetails)
            {
                foreach (var request in existingRequests)
                {
                    if (request.Id != id) // Tránh kiểm tra với chính yêu cầu hiện tại
                    {
                        foreach (var detail in request.DeviceBorrowingDetails)
                        {
                            if (IsTimeOverlapping(deviceDetail.StartDate, deviceDetail.EndDate, detail.StartDate, detail.EndDate))
                            {
                                throw new Exception($"Conflict with existing request from {detail.StartDate} to {detail.EndDate}");
                            }
                        }
                    }
                }
            }

            var updatedRequest = LabBorrowingMapper.ToModel(dto);
            updatedRequest.Id = id;
            var result = await _repository.UpdateLabBorrowingRequestAsync(updatedRequest);
            return LabBorrowingMapper.ToDto(result);
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

        public async Task<bool> DeleteLabBorrowingRequestAsync(int id)
        {
            return await _repository.DeleteLabBorrowingRequestAsync(id);
        }

        public async Task<LabBorrowingRequestDto> ApproveLabBorrowingRequestAsync(int id)
        {
            var request = await _repository.GetLabBorrowingRequestByIdAsync(id);
            if (request == null || request.Status != LabBorrowingStatus.Pending)
                return null; // Chỉ phê duyệt yêu cầu đang ở trạng thái Pending

            request.Status = LabBorrowingStatus.Approved;
            var updatedRequest = await _repository.UpdateLabBorrowingRequestAsync(request);
            return LabBorrowingMapper.ToDto(updatedRequest);
        }

        public async Task<LabBorrowingRequestDto> RejectLabBorrowingRequestAsync(int id)
        {
            var request = await _repository.GetLabBorrowingRequestByIdAsync(id);
            if (request == null || request.Status != LabBorrowingStatus.Pending)
                return null; // Chỉ từ chối yêu cầu đang ở trạng thái Pending

            request.Status = LabBorrowingStatus.Rejected;
            var updatedRequest = await _repository.UpdateLabBorrowingRequestAsync(request);
            return LabBorrowingMapper.ToDto(updatedRequest);
        }

        // Helper method để kiểm tra thời gian trùng lặp
        private bool IsTimeOverlapping(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return start1 < end2 && end1 > start2;
        }
    }
}
