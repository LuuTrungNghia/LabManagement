using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeviceBorrowingService> _logger;

        public DeviceBorrowingService(IDeviceBorrowingRepository deviceBorrowingRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, ILogger<DeviceBorrowingService> logger)
        {
            _deviceBorrowingRepository = deviceBorrowingRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context; 
            _logger = logger;
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
                if (existingRequest != null)
                {
                    // Kiểm tra yêu cầu cũ có trạng thái khác "Completed" hay không
                    if (existingRequest.Status != DeviceBorrowingStatus.Completed)
                    {
                        // Kiểm tra xem thời gian mượn của đơn mới có trùng hoặc ngay sau thời gian của đơn cũ hay không
                        foreach (var existingDetail in existingRequest.DeviceBorrowingDetails)
                        {
                            // Kiểm tra nếu thời gian mượn của đơn mới và đơn cũ có sự trùng lặp
                            if (existingDetail.DeviceItemId == detail.DeviceItemId &&
                                existingDetail.EndDate > detail.StartDate && // Đơn mới bắt đầu sau khi đơn cũ kết thúc
                                existingDetail.StartDate < detail.EndDate)   // Đơn mới kết thúc trước khi đơn cũ kết thúc
                            {
                                // Đánh dấu yêu cầu mới là Pending vì thiết bị vẫn chưa được trả lại
                                _logger.LogInformation($"DeviceItemId {detail.DeviceItemId} is still borrowed. Setting status to Pending.");

                                var deviceBorrowingRequest = new DeviceBorrowingRequest
                                {
                                    UserId = user.Id,
                                    Username = user.UserName,
                                    Description = requestDto.Description,
                                    GroupStudents = requestDto.GroupStudents?.Select(g => new GroupStudent
                                    {
                                        StudentName = g.StudentName,
                                        LectureName = g.LectureName
                                    }).ToList() ?? new List<GroupStudent>(),
                                    DeviceBorrowingDetails = requestDto.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetail
                                    {
                                        DeviceId = d.DeviceId,
                                        DeviceItemId = d.DeviceItemId,
                                        Description = d.Description,
                                        StartDate = d.StartDate,
                                        EndDate = d.EndDate,
                                    }).ToList(),
                                    Status = DeviceBorrowingStatus.Pending  // Trạng thái Pending vì thiết bị chưa trả lại
                                };

                                await _deviceBorrowingRepository.AddAsync(deviceBorrowingRequest);

                                return new DeviceBorrowingRequestDto
                                {
                                    Id = deviceBorrowingRequest.Id,
                                    Username = user.FullName,
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
                        }
                    }
                }
            }

            // Nếu không có sự trùng lặp, tạo yêu cầu mượn thiết bị với trạng thái Pending
            var deviceBorrowingRequestNew = new DeviceBorrowingRequest
            {
                UserId = user.Id,
                Username = user.UserName,
                Description = requestDto.Description,
                GroupStudents = requestDto.GroupStudents?.Select(g => new GroupStudent
                {
                    StudentName = g.StudentName,
                    LectureName = g.LectureName
                }).ToList() ?? new List<GroupStudent>(),
                DeviceBorrowingDetails = requestDto.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetail
                {
                    DeviceId = d.DeviceId,
                    DeviceItemId = d.DeviceItemId,
                    Description = d.Description,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                }).ToList(),
                Status = DeviceBorrowingStatus.Pending  // Đặt trạng thái Pending nếu không có lỗi
            };

            await _deviceBorrowingRepository.AddAsync(deviceBorrowingRequestNew);

            return new DeviceBorrowingRequestDto
            {
                Id = deviceBorrowingRequestNew.Id,
                Username = user.FullName,
                Description = deviceBorrowingRequestNew.Description,
                GroupStudents = deviceBorrowingRequestNew.GroupStudents.Select(g => new GroupStudentDto
                {
                    StudentName = g.StudentName,
                    LectureName = g.LectureName
                }).ToList(),
                DeviceBorrowingDetails = deviceBorrowingRequestNew.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetailDto
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
            // Lấy tất cả các yêu cầu mượn thiết bị từ repository
            var requests = await _deviceBorrowingRepository.GetAllAsync() ?? new List<DeviceBorrowingRequest>();

            // Trả về danh sách các yêu cầu mượn thiết bị mà không nhóm theo username
            var requestDtos = requests.Select(req => new DeviceBorrowingRequestDto
            {
                Id = req.Id,
                Username = req.Username,
                Description = req.Description,
                Status = req.Status
            }).ToList();

            return requestDtos;
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

            // Kiểm tra trạng thái của thiết bị trong yêu cầu mượn
            foreach (var detail in request.DeviceBorrowingDetails)
            {
                var deviceItem = await _context.DeviceItems.FirstOrDefaultAsync(di => di.DeviceItemId == detail.DeviceItemId);
                if (deviceItem != null)
                {
                    // Get any conflicting requests for the device item that are not the current request
                    var conflictingRequest = await _deviceBorrowingRepository.GetByDeviceItemIdAsync(detail.DeviceItemId);
                    if (conflictingRequest != null && conflictingRequest.Status != DeviceBorrowingStatus.Completed && conflictingRequest.Id != request.Id)
                    {
                        // If there is a conflicting request that is not completed, reject approval
                        return false;
                    }

                    // If the device is available, update its status to 'Borrowed'
                    deviceItem.DeviceItemStatus = DeviceItemStatus.Borrowed;
                    _logger.LogInformation($"DeviceItem {deviceItem.DeviceItemId} status updated to Borrowed.");
                }
                else
                {
                    _logger.LogWarning($"DeviceItem {detail.DeviceItemId} could not be found.");
                }
            }

            // Change the status of the request to Approved after all checks pass
            request.Status = DeviceBorrowingStatus.Approved;
            await _deviceBorrowingRepository.UpdateAsync(request);
            await _context.SaveChangesAsync(); // Save changes to the database
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
            // Lấy tất cả các yêu cầu mượn thiết bị từ repository
            var requests = await _deviceBorrowingRepository.GetDeviceBorrowingHistory(username);

            // Nếu không có yêu cầu nào thì trả về null
            if (requests == null || !requests.Any())
            {
                return null;
            }

            // Trả về tất cả lịch sử mà không nhóm dữ liệu
            var requestDtos = requests.Select(request => new DeviceBorrowingRequestHistoryDto
            {
                Id = request.Id,
                Username = request.Username,
                Description = request.Description,
                Status = request.Status,
                GroupStudents = request.GroupStudents.Select(g => new GroupStudentDto
                {
                    StudentName = g.StudentName,
                    LectureName = g.LectureName
                }).ToList(),
                DeviceBorrowingDetails = request.DeviceBorrowingDetails.Select(d => new DeviceBorrowingDetailDto
                {
                    DeviceId = d.DeviceId,
                    DeviceItemId = d.DeviceItemId,
                    Description = d.Description,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                }).ToList()
            }).ToList();

            return requestDtos;
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