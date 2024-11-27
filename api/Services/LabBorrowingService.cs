using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class LabBorrowingService : ILabBorrowingService
    {
        private readonly ILabBorrowingRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LabBorrowingService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LabBorrowingService(
            ILabBorrowingRepository repository,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger<LabBorrowingService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<LabBorrowingRequestDto> CreateLabBorrowingRequestAsync(CreateLabBorrowingRequestDto dto)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null) throw new UnauthorizedAccessException("User not authenticated.");

            // Kiểm tra nếu các thiết bị trong yêu cầu có sẵn trước khi tạo yêu cầu
            foreach (var deviceDetail in dto.DeviceBorrowingDetails)
            {
                var deviceItem = await _context.DeviceItems
                    .FirstOrDefaultAsync(di => di.DeviceItemId == deviceDetail.DeviceItemId && di.DeviceId == deviceDetail.DeviceId);

                if (deviceItem == null || deviceItem.DeviceItemStatus != DeviceItemStatus.Available)
                {
                    throw new InvalidOperationException($"Device item {deviceDetail.DeviceItemId} is not available for borrowing.");
                }
            }

            // Tạo LabBorrowingRequest mà không cần gán DeviceBorrowingRequestId ngay
            var labBorrowingRequest = new LabBorrowingRequest
            {
                UserId = user.Id,
                Username = user.UserName,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Description = dto.Description,
                Status = LabBorrowingStatus.Pending // Ban đầu thiết lập trạng thái là Pending
            };

            // Thêm LabBorrowingRequest vào cơ sở dữ liệu
            _context.LabBorrowingRequests.Add(labBorrowingRequest);

            // Lưu LabBorrowingRequest và lấy Id
            await _context.SaveChangesAsync(); 

            // Đảm bảo rằng Id của labBorrowingRequest đã được cập nhật và có giá trị hợp lệ
            if (labBorrowingRequest.Id == 0)
            {
                throw new InvalidOperationException("Failed to generate LabBorrowingRequest ID.");
            }

            // Tạo DeviceBorrowingRequest (trường hợp bạn cần có DeviceBorrowingRequest cho mỗi yêu cầu mượn thiết bị)
            var deviceBorrowingRequest = new DeviceBorrowingRequest
            {
                Username = user.UserName,
                Description = "Borrowing request for devices",
                Status = DeviceBorrowingStatus.Pending,
                UserId = user.Id,
                LabBorrowingRequestId = labBorrowingRequest.Id  // Liên kết DeviceBorrowingRequest với LabBorrowingRequest
            };

            _context.DeviceBorrowingRequests.Add(deviceBorrowingRequest);
            await _context.SaveChangesAsync(); // Lưu DeviceBorrowingRequest và lấy ID

            // Bây giờ tạo các DeviceBorrowingDetails với Id yêu cầu đúng
            foreach (var deviceDetail in dto.DeviceBorrowingDetails)
            {
                var deviceBorrowingDetail = new DeviceBorrowingDetail
                {
                    DeviceId = deviceDetail.DeviceId,
                    DeviceItemId = deviceDetail.DeviceItemId,
                    Description = deviceDetail.Description,
                    LabBorrowingRequestId = labBorrowingRequest.Id,  // Gán LabBorrowingRequestId
                    DeviceBorrowingRequestId = deviceBorrowingRequest.Id  // Gán DeviceBorrowingRequestId
                };

                // Thêm DeviceBorrowingDetail vào cơ sở dữ liệu
                _context.DeviceBorrowingDetails.Add(deviceBorrowingDetail);
            }

            // Lưu các thay đổi cho DeviceBorrowingDetails
            await _context.SaveChangesAsync();

            // Trả về LabBorrowingRequestDto với các chi tiết
            return new LabBorrowingRequestDto
            {
                Id = labBorrowingRequest.Id,
                Username = labBorrowingRequest.Username,
                StartDate = labBorrowingRequest.StartDate,
                EndDate = labBorrowingRequest.EndDate,
                Description = labBorrowingRequest.Description,
                Status = labBorrowingRequest.Status,
                DeviceBorrowingDetails = labBorrowingRequest.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetailDto
                {
                    DeviceId = d.DeviceId,
                    DeviceItemId = d.DeviceItemId,
                    Description = d.Description
                }).ToList()
            };
        }

        // Lấy thông tin yêu cầu mượn phòng thí nghiệm theo ID
        public async Task<LabBorrowingRequestDto> GetLabBorrowingRequestByIdAsync(int id)
        {
            var request = await _repository.GetLabBorrowingRequestByIdAsync(id);
            if (request == null) return null;

            return new LabBorrowingRequestDto
            {
                Id = request.Id,
                Username = request.Username,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Description = request.Description,
                Status = request.Status,
                DeviceBorrowingDetails = request.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetailDto
                {
                    DeviceId = d.DeviceId,
                    DeviceItemId = d.DeviceItemId,
                    Description = d.Description
                }).ToList()
            };
        }

        // Lấy tất cả yêu cầu mượn phòng thí nghiệm
        public async Task<IEnumerable<LabBorrowingRequestDto>> GetAllLabBorrowingRequestsAsync()
        {
            var requests = await _repository.GetAllLabBorrowingRequestsAsync();

            return requests.Select(request => new LabBorrowingRequestDto
            {
                Id = request.Id,
                Username = request.Username,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Description = request.Description,
                Status = request.Status,
                DeviceBorrowingDetails = request.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetailDto
                {
                    DeviceId = d.DeviceId,
                    DeviceItemId = d.DeviceItemId,
                    Description = d.Description
                }).ToList()
            }).ToList();
        }

        // Cập nhật yêu cầu mượn phòng thí nghiệm
        public async Task<LabBorrowingRequestDto> UpdateLabBorrowingRequestAsync(int id, UpdateLabBorrowingRequestDto dto)
        {
            var request = await _repository.GetLabBorrowingRequestByIdAsync(id);
            if (request == null) throw new KeyNotFoundException("Lab Borrowing Request not found.");

            // Validate device status for updated request
            foreach (var deviceDetail in dto.DeviceBorrowingDetails)
            {
                var deviceItem = await _context.DeviceItems
                    .FirstOrDefaultAsync(di => di.DeviceItemId == deviceDetail.DeviceItemId && di.DeviceId == deviceDetail.DeviceId);

                if (deviceItem == null || deviceItem.DeviceItemStatus != DeviceItemStatus.Available)
                {
                    throw new InvalidOperationException($"Device item {deviceDetail.DeviceItemId} is not available for borrowing.");
                }
            }

            request.Description = dto.Description;
            request.StartDate = dto.StartDate;
            request.EndDate = dto.EndDate;
            request.DeviceBorrowingDetails = dto.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetail
            {
                DeviceId = d.DeviceId,
                DeviceItemId = d.DeviceItemId,
                Description = d.Description
            }).ToList();

            var updatedRequest = await _repository.UpdateLabBorrowingRequestAsync(request);

            return new LabBorrowingRequestDto
            {
                Id = updatedRequest.Id,
                Username = updatedRequest.Username,
                StartDate = updatedRequest.StartDate,
                EndDate = updatedRequest.EndDate,
                Description = updatedRequest.Description,
                Status = updatedRequest.Status,
                DeviceBorrowingDetails = updatedRequest.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetailDto
                {
                    DeviceId = d.DeviceId,
                    DeviceItemId = d.DeviceItemId,
                    Description = d.Description
                }).ToList()
            };
        }

        // Xóa yêu cầu mượn phòng thí nghiệm
        public async Task<bool> DeleteLabBorrowingRequestAsync(int id)
        {
            return await _repository.DeleteLabBorrowingRequestAsync(id);
        }

        // Phê duyệt yêu cầu mượn phòng thí nghiệm
        public async Task<bool> ApproveLabBorrowingRequestAsync(int id)
        {
            var request = await _repository.GetLabBorrowingRequestByIdAsync(id);
            if (request == null) return false;

            request.Status = LabBorrowingStatus.Approved;
            await _repository.UpdateLabBorrowingRequestAsync(request);
            return true;
        }

        // Từ chối yêu cầu mượn phòng thí nghiệm
        public async Task<bool> RejectLabBorrowingRequestAsync(int id)
        {
            var request = await _repository.GetLabBorrowingRequestByIdAsync(id);
            if (request == null) return false;

            request.Status = LabBorrowingStatus.Rejected;
            await _repository.UpdateLabBorrowingRequestAsync(request);
            return true;
        }

        public async Task<IEnumerable<LabBorrowingRequestHistoryDto>> GetLabBorrowingHistoryAsync(string username)
        {
            var requests = await _context.LabBorrowingRequests
                .Include(r => r.DeviceBorrowingDetails)
                .Where(r => r.Username == username)
                .ToListAsync();

            if (requests == null || !requests.Any())
            {
                return null; // No history found
            }

            return requests.Select(r => new LabBorrowingRequestHistoryDto
            {
                Id = r.Id,
                Username = r.Username,
                Description = r.Description,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Status = r.Status,
                DeviceBorrowingDetails = r.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetailDto
                {
                    DeviceId = d.DeviceId,
                    DeviceItemId = d.DeviceItemId,
                    Description = d.Description
                }).ToList()
            }).ToList();
        }
    }
}
