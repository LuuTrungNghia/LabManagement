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

            // Check if devices in the request are available
            foreach (var deviceDetail in dto.DeviceBorrowingDetails)
            {
                var deviceItem = await _context.DeviceItems
                    .FirstOrDefaultAsync(di => di.DeviceItemId == deviceDetail.DeviceItemId && di.DeviceId == deviceDetail.DeviceId);

                if (deviceItem == null || deviceItem.DeviceItemStatus != DeviceItemStatus.Available)
                {
                    throw new InvalidOperationException($"Device item {deviceDetail.DeviceItemId} is not available for borrowing.");
                }

                // Check for overlapping borrow requests
                var overlappingRequest = await _context.LabBorrowingRequests
                    .Where(r => r.DeviceBorrowingDetails.Any(d => d.DeviceItemId == deviceDetail.DeviceItemId))
                    .Where(r => (r.StartDate < dto.EndDate && r.EndDate > dto.StartDate) && (r.Status == LabBorrowingStatus.Pending || r.Status == LabBorrowingStatus.Approved))
                    .FirstOrDefaultAsync();

                if (overlappingRequest != null)
                {
                    throw new InvalidOperationException($"Device item {deviceDetail.DeviceItemId} is already borrowed during the requested time frame.");
                }
            }

            // Create LabBorrowingRequest without DeviceBorrowingRequestId
            var labBorrowingRequest = new LabBorrowingRequest
            {
                UserId = user.Id,
                Username = user.UserName,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Description = dto.Description,
                Status = LabBorrowingStatus.Pending // Initially set to Pending
            };

            // Add LabBorrowingRequest to database
            _context.LabBorrowingRequests.Add(labBorrowingRequest);
            await _context.SaveChangesAsync();

            if (labBorrowingRequest.Id == 0)
            {
                throw new InvalidOperationException("Failed to generate LabBorrowingRequest ID.");
            }

            // Create DeviceBorrowingRequest and associate with LabBorrowingRequest
            var deviceBorrowingRequest = new DeviceBorrowingRequest
            {
                Username = user.UserName,
                Description = "Borrowing request for devices",
                Status = DeviceBorrowingStatus.Pending,
                UserId = user.Id,
                LabBorrowingRequestId = labBorrowingRequest.Id
            };

            // Add DeviceBorrowingRequest
            _context.DeviceBorrowingRequests.Add(deviceBorrowingRequest);
            await _context.SaveChangesAsync();

            // Create DeviceBorrowingDetails and associate with LabBorrowingRequest and DeviceBorrowingRequest
            foreach (var deviceDetail in dto.DeviceBorrowingDetails)
            {
                var deviceBorrowingDetail = new DeviceBorrowingDetail
                {
                    DeviceId = deviceDetail.DeviceId,
                    DeviceItemId = deviceDetail.DeviceItemId,
                    Description = deviceDetail.Description,
                    LabBorrowingRequestId = labBorrowingRequest.Id,
                    DeviceBorrowingRequestId = deviceBorrowingRequest.Id
                };

                _context.DeviceBorrowingDetails.Add(deviceBorrowingDetail);
            }

            await _context.SaveChangesAsync();

            // Return LabBorrowingRequestDto with details
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

            // Update LabBorrowingRequest details
            request.Description = dto.Description;
            request.StartDate = dto.StartDate;
            request.EndDate = dto.EndDate;

            // Update DeviceBorrowingDetails and associate with DeviceBorrowingRequest
            request.DeviceBorrowingDetails = dto.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetail
            {
                DeviceId = d.DeviceId,
                DeviceItemId = d.DeviceItemId,
                Description = d.Description,
                DeviceBorrowingRequestId = request.DeviceBorrowingRequests.First().Id // Ensure valid ID
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
            var request = await _context.LabBorrowingRequests
                .Include(r => r.DeviceBorrowingRequests) // Include related requests
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return false;

            // Delete dependent records (if any)
            foreach (var borrowingRequest in request.DeviceBorrowingRequests)
            {
                _context.DeviceBorrowingRequests.Remove(borrowingRequest);
            }

            // Delete the LabBorrowingRequest
            _context.LabBorrowingRequests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }

        // Phê duyệt yêu cầu mượn phòng thí nghiệm
        public async Task<bool> ApproveLabBorrowingRequestAsync(int id)
        {
            var request = await _repository.GetLabBorrowingRequestByIdAsync(id);
            if (request == null) return false;

            // Check the status of the devices before approval
            foreach (var deviceDetail in request.DeviceBorrowingDetails)
            {
                var deviceItem = await _context.DeviceItems
                    .FirstOrDefaultAsync(di => di.DeviceItemId == deviceDetail.DeviceItemId && di.DeviceId == deviceDetail.DeviceId);

                // Check if the device item exists and if it's available
                if (deviceItem == null || deviceItem.DeviceItemStatus != DeviceItemStatus.Available)
                {
                    // If the device is not available or already borrowed, prevent approval
                    throw new InvalidOperationException($"Device item {deviceDetail.DeviceItemId} is not available for borrowing or has not been returned.");
                }

                // Check if the device item is already borrowed during the requested period
                var overlappingRequest = await _context.LabBorrowingRequests
                    .Where(r => r.DeviceBorrowingDetails.Any(d => d.DeviceItemId == deviceDetail.DeviceItemId))
                    .Where(r => (r.StartDate < request.EndDate && r.EndDate > request.StartDate) && (r.Status == LabBorrowingStatus.Approved || r.Status == LabBorrowingStatus.Pending))
                    .FirstOrDefaultAsync();

                // If there is any overlapping request for this device, reject the approval
                if (overlappingRequest != null)
                {
                    throw new InvalidOperationException($"Device item {deviceDetail.DeviceItemId} is already borrowed during the requested time frame.");
                }

                // Update the device status to 'Borrowed' (or a similar status)
                deviceItem.DeviceItemStatus = DeviceItemStatus.Borrowed;
            }

            // If all devices are available and no overlapping requests, approve the lab borrowing request
            request.Status = LabBorrowingStatus.Approved;
            await _context.SaveChangesAsync();

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
