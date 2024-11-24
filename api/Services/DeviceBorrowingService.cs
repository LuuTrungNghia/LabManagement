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

            // Validate the device borrowing details
            if (requestDto.DeviceBorrowingDetails == null || !requestDto.DeviceBorrowingDetails.Any())
            {
                throw new ArgumentException("DeviceBorrowingDetails cannot be null or empty.");
            }

            // Check if devices are available
            foreach (var detail in requestDto.DeviceBorrowingDetails)
            {
                var existingRequest = await _deviceBorrowingRepository.GetByDeviceItemIdAsync(detail.DeviceItemId);
                if (existingRequest != null && existingRequest.Status != DeviceBorrowingStatus.Completed)
                {
                    throw new ArgumentException($"DeviceItemId {detail.DeviceItemId} is already borrowed or pending.");
                }
            }

            // Validate student usernames
            if (requestDto.StudentUsernames != null && requestDto.StudentUsernames.Any())
            {
                foreach (var studentUsername in requestDto.StudentUsernames)
                {
                    var student = await _userManager.FindByNameAsync(studentUsername);
                    if (student == null)
                    {
                        throw new ArgumentException($"Student with username '{studentUsername}' does not exist.");
                    }
                }
            }

            // Validate lecturer username
            if (!string.IsNullOrEmpty(requestDto.LecturerUsername))
            {
                var lecturer = await _userManager.FindByNameAsync(requestDto.LecturerUsername);
                if (lecturer == null)
                {
                    throw new ArgumentException($"Lecturer with username '{requestDto.LecturerUsername}' does not exist.");
                }
            }

            // Create device borrowing request
            var deviceBorrowingRequest = new DeviceBorrowingRequest
            {
                Username = user.UserName,
                UserId = user.Id,
                Description = requestDto.Description,                
                DeviceBorrowingDetails = requestDto.DeviceBorrowingDetails.Select(detail => new DeviceBorrowingDetail
                {
                    DeviceId = detail.DeviceId,
                    DeviceItemId = detail.DeviceItemId,
                    Description = detail.Description,
                    StartDate = detail.StartDate,
                    EndDate = detail.EndDate,
                }).ToList(),
                StudentUsernames = requestDto.StudentUsernames,
                LecturerUsername = requestDto.LecturerUsername
            };

            // Save the request to the database
            await _deviceBorrowingRepository.AddAsync(deviceBorrowingRequest);

            return new DeviceBorrowingRequestDto
            {
                Id = deviceBorrowingRequest.Id,
                Username = deviceBorrowingRequest.Username,
                Description = deviceBorrowingRequest.Description,
                
                DeviceBorrowingDetails = deviceBorrowingRequest.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetailDto
                {
                    DeviceId = d.DeviceId,
                    DeviceItemId = d.DeviceItemId,
                    Description = d.Description,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                }).ToList()
            };
        }

        public async Task<DeviceBorrowingRequest> CheckIfDeviceIsAvailable(int deviceItemId)
        {
            var existingRequest = await _deviceBorrowingRepository.GetByDeviceItemIdAsync(deviceItemId);
            return existingRequest;
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
                    Status = group.First().Status, // Assuming status is the same for all requests in the group
                    DeviceBorrowingDetails = group
                        .SelectMany(r => r.DeviceBorrowingDetails) // Flatten all the device borrowing details for the group
                        .Select(d => new DeviceBorrowingDetailDto
                        {
                            DeviceId = d.DeviceId,
                            DeviceItemId = d.DeviceItemId,
                            Description = d.Description,
                            StartDate = d.StartDate, // Assuming the StartDate should be the same for grouped requests
                            EndDate = d.EndDate, // Same for EndDate
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
                Status = request.Status, // The approval status of the request
                DeviceBorrowingDetails = request.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetailDto
                {
                    DeviceId = d.DeviceId,
                    DeviceItemId = d.DeviceItemId,
                    Description = d.Description,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                }).ToList()
            };
        }

        public async Task<DeviceBorrowingRequestDto> UpdateDeviceBorrowingRequest(int id, UpdateDeviceBorrowingRequestDto requestDto)
        {
            var request = await _deviceBorrowingRepository.GetByIdAsync(id);
            if (request == null)
                return null;

            request.Description = requestDto.Description;            
            // Clear existing details and add new ones
            request.DeviceBorrowingDetails.Clear();
            foreach (var detail in requestDto.DeviceBorrowingDetails)
            {
                request.DeviceBorrowingDetails.Add(new DeviceBorrowingDetail
                {
                    DeviceId = detail.DeviceId,
                    DeviceItemId = detail.DeviceItemId,
                    Description = detail.Description,
                    StartDate = detail.StartDate,
                    EndDate = detail.EndDate,
                });
            }

            await _deviceBorrowingRepository.UpdateAsync(request);

            return new DeviceBorrowingRequestDto
            {
                Id = request.Id,
                Username = request.Username,
                Description = request.Description,
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
            var requests = await _deviceBorrowingRepository.GetDeviceBorrowingHistory(username);

            if (requests == null || !requests.Any())
            {
                return null;
            }

            var approvedRequests = requests
                .Where(request => request.Status == DeviceBorrowingStatus.Approved)
                .ToList();

            var groupedRequests = approvedRequests
                .GroupBy(request => request.Id)
                .Select(group => new DeviceBorrowingRequestHistoryDto
                {
                    Id = group.Key,
                    Username = group.First().Username,
                    Description = group.First().Description,                    
                    Status = group.First().Status,
                    DeviceBorrowingDetails = group
                        .SelectMany(request => request.DeviceBorrowingDetails)
                        .Select(d => new DeviceBorrowingDetailDto
                        {
                            DeviceId = d.DeviceId,
                            DeviceItemId = d.DeviceItemId,
                            Description = d.Description,
                            StartDate = d.StartDate,
                            EndDate = d.EndDate,
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