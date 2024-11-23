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
                throw new Exception("User not authenticated or not found.");
            }

            if (requestDto.DeviceBorrowingDetails == null || !requestDto.DeviceBorrowingDetails.Any())
            {
                throw new ArgumentException("DeviceBorrowingDetails cannot be null or empty.");
            }

            // Validate each device item
            foreach (var detail in requestDto.DeviceBorrowingDetails)
            {
                var existingRequest = await _deviceBorrowingRepository.GetByDeviceItemIdAsync(detail.DeviceItemId);
                if (existingRequest != null && existingRequest.Status != DeviceBorrowingStatus.Completed)
                {
                    throw new ArgumentException($"DeviceItemId {detail.DeviceItemId} is already borrowed or pending.");
                }
            }

            // Create borrowing request
            var deviceBorrowingRequest = new DeviceBorrowingRequest
            {
                Username = user.UserName,
                UserId = user.Id,  // Make sure UserId is set correctly here
                Description = requestDto.Description,
                FromDate = requestDto.FromDate,
                ToDate = requestDto.ToDate,
                DeviceBorrowingDetails = requestDto.DeviceBorrowingDetails.Select(detail => new DeviceBorrowingDetail
                {
                    DeviceId = detail.DeviceId,
                    DeviceItemId = detail.DeviceItemId,
                    Description = detail.Description
                }).ToList()
            };

            await _deviceBorrowingRepository.AddAsync(deviceBorrowingRequest);

            // Map to DTO to return
            return new DeviceBorrowingRequestDto
            {
                Id = deviceBorrowingRequest.Id,
                Username = deviceBorrowingRequest.Username,
                Description = deviceBorrowingRequest.Description,
                FromDate = deviceBorrowingRequest.FromDate,
                ToDate = deviceBorrowingRequest.ToDate,
                DeviceBorrowingDetails = deviceBorrowingRequest.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetailDto
                {
                    DeviceId = d.DeviceId,
                    DeviceItemId = d.DeviceItemId,
                    Description = d.Description
                }).ToList()
            };
        }

        public async Task<List<DeviceBorrowingRequestDto>> GetDeviceBorrowingRequests()
        {
            // Fetch all device borrowing requests, including details for each request
            var requests = await _deviceBorrowingRepository.GetAllAsync();

            // Group requests by username (or userId) and merge devices in the borrowing details
            var groupedRequests = requests
                .GroupBy(r => r.Username) // Group by Username or UserId
                .Select(group => new DeviceBorrowingRequestDto
                {
                    // Assign the ID from the first request in the group
                    Id = group.First().Id, // Use the first request's ID for the grouped entry
                    Username = group.Key, // Username will be the key of the group
                    Description = group.First().Description, // Assuming all requests in a group have the same description
                    FromDate = group.Min(r => r.FromDate), // Assuming the FromDate should be the same for grouped requests
                    ToDate = group.Max(r => r.ToDate), // Same for ToDate
                    Status = group.First().Status, // Assuming status is the same for all requests in the group
                    DeviceBorrowingDetails = group
                        .SelectMany(r => r.DeviceBorrowingDetails) // Flatten all the device borrowing details for the group
                        .Select(d => new DeviceBorrowingDetailDto
                        {
                            DeviceId = d.DeviceId,
                            DeviceItemId = d.DeviceItemId,
                            Description = d.Description
                        }).ToList()
                }).ToList();

            return groupedRequests;
        }

        public async Task<DeviceBorrowingRequestDto> GetDeviceBorrowingRequestById(int id)
        {
            var request = await _deviceBorrowingRepository.GetByIdAsync(id);
            if (request == null)
            {
                return null;
            }

            // Map the response to DTO and include the device details
            return new DeviceBorrowingRequestDto
            {
                Id = request.Id,
                Username = request.Username,
                Description = request.Description,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Status = request.Status, // The approval status of the request
                DeviceBorrowingDetails = request.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetailDto
                {
                    DeviceId = d.DeviceId,
                    DeviceItemId = d.DeviceItemId,
                    Description = d.Description
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

        public async Task<bool> RejectDeviceBorrowingRequest(int id)
        {
            var borrowingRequest = await _deviceBorrowingRepository.GetByIdAsync(id);
            if (borrowingRequest == null || borrowingRequest.Status != DeviceBorrowingStatus.Pending)
            {
                return false;  
            }

            borrowingRequest.Status = DeviceBorrowingStatus.Rejected; 
            await _deviceBorrowingRepository.UpdateAsync(borrowingRequest);
            return true;
        }

       public async Task<List<DeviceBorrowingRequestHistoryDto>> GetDeviceBorrowingHistory(string username)
        {
            // Fetch the device borrowing requests by username and their details
            var requests = await _deviceBorrowingRepository.GetDeviceBorrowingHistory(username);

            if (requests == null || !requests.Any())
            {
                return null; // Or throw an exception if needed
            }

            // Filter only approved requests
            var approvedRequests = requests
                .Where(request => request.Status == DeviceBorrowingStatus.Approved)
                .ToList();

            if (!approvedRequests.Any())
            {
                return null; // No approved requests found
            }

            // Group the requests by RequestId and merge the devices in each request
            var groupedRequests = approvedRequests
                .GroupBy(request => request.Id) // Group by the RequestId
                .Select(group => new DeviceBorrowingRequestHistoryDto
                {
                    Id = group.Key,
                    Username = group.First().Username,
                    Description = group.First().Description,
                    FromDate = group.Min(request => request.FromDate), // Take the earliest start date
                    ToDate = group.Max(request => request.ToDate), // Take the latest end date
                    Status = group.First().Status, // All requests in the group should have the same status
                    DeviceBorrowingDetails = group
                        .SelectMany(request => request.DeviceBorrowingDetails) // Flatten the list of device borrowing details
                        .Select(d => new DeviceBorrowingDetailDto
                        {
                            DeviceId = d.DeviceId,
                            DeviceItemId = d.DeviceItemId,
                            Description = d.Description
                        }).ToList()
                }).ToList();

            return groupedRequests;
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
