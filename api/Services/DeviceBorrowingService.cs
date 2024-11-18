using api.Dtos;
using api.Interfaces;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
   public class DeviceBorrowingService : IDeviceBorrowingService
    {
        private readonly IDeviceBorrowingRepository _deviceBorrowingRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeviceBorrowingService(IDeviceBorrowingRepository deviceBorrowingRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _deviceBorrowingRepository = deviceBorrowingRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DeviceBorrowingRequestDto> CreateDeviceBorrowingRequest(CreateDeviceBorrowingRequestDto requestDto)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
            {
                return null; // User not found
            }

            var deviceBorrowingRequest = new DeviceBorrowingRequest
            {
                Username = requestDto.Username,
                Description = requestDto.Description,
                FromDate = requestDto.FromDate,
                ToDate = requestDto.ToDate,
                UserId = user.Id, // Set the UserId from the authenticated user
                Status = DeviceBorrowingStatus.Pending, // Default status
                DeviceBorrowingDetails = requestDto.DeviceBorrowingDetails.Select(detail => new DeviceBorrowingDetail
                {
                    DeviceId = detail.DeviceId,
                    DeviceItemId = detail.DeviceItemId,
                    Description = detail.Description
                }).ToList() // Map details
            };

            // Add the request to the database
            await _deviceBorrowingRepository.AddAsync(deviceBorrowingRequest);

            // Map to DTO before returning
            return new DeviceBorrowingRequestDto
            {
                Id = deviceBorrowingRequest.Id,
                Username = deviceBorrowingRequest.Username,
                Description = deviceBorrowingRequest.Description,
                FromDate = deviceBorrowingRequest.FromDate,
                ToDate = deviceBorrowingRequest.ToDate,
                Status = deviceBorrowingRequest.Status,
                DeviceBorrowingDetails = deviceBorrowingRequest.DeviceBorrowingDetails.Select(detail => new DeviceBorrowingDetailDto
                {
                    DeviceId = detail.DeviceId,
                    DeviceItemId = detail.DeviceItemId,
                    Description = detail.Description
                }).ToList()
            };
        }

        public async Task<List<DeviceBorrowingRequestDto>> GetDeviceBorrowingRequests()
        {
            var requests = await _deviceBorrowingRepository.GetAllAsync();
            return requests.Select(request => new DeviceBorrowingRequestDto
            {
                Id = request.Id,
                Username = request.Username,
                Description = request.Description,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Status = request.Status,
                DeviceBorrowingDetails = request.DeviceBorrowingDetails.Select(detail => new DeviceBorrowingDetailDto
                {
                    DeviceId = detail.DeviceId,
                    DeviceItemId = detail.DeviceItemId,
                    Description = detail.Description
                }).ToList()
            }).ToList();
        }

        public async Task<DeviceBorrowingRequestDto> GetDeviceBorrowingRequestById(int id)
        {
            var request = await _deviceBorrowingRepository.GetByIdAsync(id);
            if (request == null)
                return null;

            return new DeviceBorrowingRequestDto
            {
                Id = request.Id,
                Username = request.Username,
                Description = request.Description,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Status = request.Status,
                DeviceBorrowingDetails = request.DeviceBorrowingDetails.Select(detail => new DeviceBorrowingDetailDto
                {
                    DeviceId = detail.DeviceId,
                    DeviceItemId = detail.DeviceItemId,
                    Description = detail.Description
                }).ToList()
            };
        }

        public async Task<DeviceBorrowingRequestDto> UpdateDeviceBorrowingRequest(int id, UpdateDeviceBorrowingRequestDto requestDto)
        {
            var request = await _deviceBorrowingRepository.GetByIdAsync(id);
            if (request == null)
                return null;

            request.Description = requestDto.Description;
            request.FromDate = requestDto.FromDate;
            request.ToDate = requestDto.ToDate;

            // Clear existing details and add new ones
            request.DeviceBorrowingDetails.Clear();
            foreach (var detail in requestDto.DeviceBorrowingDetails)
            {
                request.DeviceBorrowingDetails.Add(new DeviceBorrowingDetail
                {
                    DeviceId = detail.DeviceId,
                    DeviceItemId = detail.DeviceItemId,
                    Description = detail.Description
                });
            }

            await _deviceBorrowingRepository.UpdateAsync(request);

            return new DeviceBorrowingRequestDto
            {
                Id = request.Id,
                Username = request.Username,
                Description = request.Description,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Status = request.Status,
                DeviceBorrowingDetails = requestDto.DeviceBorrowingDetails
            };
        }

        public async Task<bool> ApproveDeviceBorrowingRequest(int id)
        {
            var request = await _deviceBorrowingRepository.GetByIdAsync(id);
            if (request == null || request.Status == DeviceBorrowingStatus.Approved)
            {
                return false;
            }

            request.Status = DeviceBorrowingStatus.Approved;
            await _deviceBorrowingRepository.UpdateAsync(request);
            return true;
        }

        public async Task<List<DeviceBorrowingRequestHistoryDto>> GetDeviceBorrowingHistory(string username)
        {
            var requests = await _deviceBorrowingRepository.GetByUsernameAsync(username);
            return requests.Select(request => new DeviceBorrowingRequestHistoryDto
            {
                Id = request.Id,
                Description = request.Description,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Status = request.Status
            }).ToList();
        }

        public async Task<bool> ReturnDevice(DeviceReturnDto deviceReturnDto)
        {
            var request = await _deviceBorrowingRepository.GetByDeviceItemIdAsync(deviceReturnDto.DeviceItemId);
            if (request == null || request.Status != DeviceBorrowingStatus.Approved)
            {
                return false;
            }

            request.Status = DeviceBorrowingStatus.Completed;  // Ensure 'Completed' is in the enum
            await _deviceBorrowingRepository.UpdateAsync(request);

            return true;
        }
    }
}
