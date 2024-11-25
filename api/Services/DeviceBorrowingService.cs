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

            foreach (var detail in requestDto.DeviceBorrowingDetails)
            {
                var existingRequest = await _deviceBorrowingRepository.GetByDeviceItemIdAsync(detail.DeviceItemId);
                if (existingRequest != null && existingRequest.Status != DeviceBorrowingStatus.Completed)
                {
                    throw new ArgumentException($"DeviceItemId {detail.DeviceItemId} is already borrowed or pending.");
                }
            }

            var deviceBorrowingRequest = new DeviceBorrowingRequest
            {
                UserId = user.Id,
                Description = requestDto.Description,
                GroupStudents = requestDto.GroupStudents?.Select(g => new GroupStudent
                {
                    StudentName = g.StudentName,
                    LectureName = g.LectureName
                }).ToList() ?? new List<GroupStudent>(),
                DeviceBorrowingDetails = requestDto.DeviceBorrowingDetails.Select(detail => new DeviceBorrowingDetail
                {
                    DeviceId = detail.DeviceId,
                    DeviceItemId = detail.DeviceItemId,
                    Description = detail.Description,
                    StartDate = detail.StartDate,
                    EndDate = detail.EndDate,
                }).ToList(),
            };

            await _deviceBorrowingRepository.AddAsync(deviceBorrowingRequest);

            return new DeviceBorrowingRequestDto
            {
                Id = deviceBorrowingRequest.Id,
                Username = user.UserName, // Lấy username từ token
                Description = deviceBorrowingRequest.Description,
                GroupStudents = deviceBorrowingRequest.GroupStudents.Select(g => new GroupStudentDto
                {
                    StudentName = g.StudentName,
                    LectureName = g.LectureName
                }).ToList(),
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

            // Xử lý GroupStudents và trả về mảng rỗng nếu không có dữ liệu
            return new DeviceBorrowingRequestDto
            {
                Id = request.Id,
                Username = request.Username,
                Description = request.Description,
                Status = request.Status,
                GroupStudents = request.GroupStudents?.Select(g => new GroupStudentDto
                {
                    StudentName = g.StudentName,
                    LectureName = g.LectureName
                }).ToList() ?? new List<GroupStudentDto>(),  // Nếu không có dữ liệu, trả về mảng rỗng
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

            // Clear existing details và thêm mới
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

            // Cập nhật lại GroupStudents nếu có dữ liệu
            request.GroupStudents = requestDto.GroupStudents?.Select(g => new GroupStudent
            {
                StudentName = g.StudentName,
                LectureName = g.LectureName
            }).ToList() ?? new List<GroupStudent>(); // Nếu không có GroupStudents, trả về mảng rỗng

            await _deviceBorrowingRepository.UpdateAsync(request);

            return new DeviceBorrowingRequestDto
            {
                Id = request.Id,
                Username = request.Username,
                Description = request.Description,
                Status = request.Status,
                GroupStudents = request.GroupStudents?.Select(g => new GroupStudentDto
                {
                    StudentName = g.StudentName,
                    LectureName = g.LectureName
                }).ToList(),
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
                    GroupStudents = group
                        .SelectMany(r => r.GroupStudents)
                        .Select(g => new GroupStudentDto
                        {
                            StudentName = g.StudentName,
                            LectureName = g.LectureName
                        }).ToList(),
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
        public async Task<bool> DeleteDeviceBorrowingRequest(int id)
        {
            var request = await _deviceBorrowingRepository.GetByIdAsync(id);
            if (request == null)
            {
                return false; // Không tìm thấy yêu cầu
            }

            await _deviceBorrowingRepository.DeleteAsync(request);
            return true;
        }
    }
}