using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Dtos;
using api.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceBorrowingController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDeviceBorrowingService _deviceBorrowingService;

        public DeviceBorrowingController(IDeviceBorrowingService deviceBorrowingService)
        {
            _deviceBorrowingService = deviceBorrowingService;
        }

        // Get all device borrowing requests
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetDeviceBorrowingRequests()
        {
            var requests = await _deviceBorrowingService.GetDeviceBorrowingRequests();

            if (requests == null || !requests.Any())
            {
                return NotFound("No borrowing requests found.");
            }

            // Trả về danh sách các yêu cầu mượn thiết bị mà không nhóm theo username
            var response = requests.Select(req => new
            {
                Id = req.Id,
                Username = req.Username,
                Description = req.Description,
                Status = req.Status
            }).ToList();

            return Ok(response);
        }

        // Get a specific device borrowing request by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "admin, student, lecturer")]
        public async Task<IActionResult> GetDeviceBorrowingRequest(int id)
        {
            var request = await _deviceBorrowingService.GetDeviceBorrowingRequestById(id);
            if (request == null || (User.IsInRole("student") && request.Username != User.Identity.Name))
            {
                return NotFound("Request not found or access denied.");
            }

            var response = new
            {
                Id = request.Id,
                Username = request.Username,
                Description = request.Description,
                Status = request.Status,
                GroupStudents = request.GroupStudents.Select(g => new
                {
                    StudentName = g.StudentName,
                    LectureName = g.LectureName
                }).ToList(),
                DeviceBorrowingDetails = request.DeviceBorrowingDetails
                    .GroupBy(d => new { d.DeviceId, d.DeviceItemId })
                    .Select(group => new
                    {
                        DeviceId = group.Key.DeviceId,
                        DeviceItemId = group.Key.DeviceItemId,
                        Description = string.Join(", ", group.Select(d => d.Description)),
                        StartDate = group.Min(d => d.StartDate),
                        EndDate = group.Max(d => d.EndDate), 
                    })
                    .ToList()
            };

            return Ok(response);
        }

        // Create a new device borrowing request
        [HttpPost("create")]
        [Authorize(Roles = "admin, student, lecturer")]
        public async Task<IActionResult> CreateDeviceBorrowingRequest([FromBody] CreateDeviceBorrowingRequestDto requestDto)
        {
            try
            {
                // foreach (var detail in requestDto.DeviceBorrowingDetails)
                // {
                //     var existingRequest = await _deviceBorrowingService.CheckIfDeviceIsAvailable(detail.DeviceItemId);
                //     if (existingRequest != null && existingRequest.Status != DeviceBorrowingStatus.Completed)
                //     {
                //         return BadRequest($"Device {detail.DeviceItemId} is already borrowed or not available.");
                //     }
                // }

                // Create borrowing request
                var result = await _deviceBorrowingService.CreateDeviceBorrowingRequest(requestDto);
                if (result == null)
                {
                    return BadRequest("Could not create borrowing request. Please ensure the devices are available.");
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        // Update a device borrowing request
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateDeviceBorrowingRequest(int id, [FromBody] UpdateDeviceBorrowingRequestDto requestDto)
        {
            var result = await _deviceBorrowingService.UpdateDeviceBorrowingRequest(id, requestDto);
            if (result == null)
            {
                return BadRequest("Could not update borrowing request.");
            }
            return Ok(result);
        }

        // Approve a device borrowing request
        [HttpPost("{id}/approve")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveDeviceBorrowingRequest(int id)
        {
            var result = await _deviceBorrowingService.ApproveDeviceBorrowingRequest(id);
            if (result)
            {
                return Ok("Borrowing request approved.");
            }
            return BadRequest("Could not approve borrowing request.");
        }

        // Reject a device borrowing request
        [HttpPost("{id}/reject")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RejectDeviceBorrowingRequest(int id)
        {
            var result = await _deviceBorrowingService.RejectDeviceBorrowingRequest(id);
            if (result)
            {
                return Ok("Borrowing request rejected.");
            }
            return BadRequest("Could not reject borrowing request.");
        }

        // Return a borrowed device
        [HttpPost("return")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ReturnDevice([FromBody] DeviceReturnDto deviceReturnDto)
        {
            var result = await _deviceBorrowingService.ReturnDevice(deviceReturnDto);
            if (result)
            {
                return Ok("Device returned successfully.");
            }
            return BadRequest("Could not return device.");
        }

        // Get device borrowing history for a specific user
        [HttpGet("history/{username}")]
        [Authorize(Roles = "admin, student, lecturer")]
        public async Task<IActionResult> GetDeviceBorrowingHistory(string username)
        {
            // Kiểm tra quyền người dùng
            if (!User.IsInRole("admin") && username != User.Identity.Name)
            {
                return Forbid("You are not authorized to view this history.");
            }

            // Lấy tất cả lịch sử mượn thiết bị
            var history = await _deviceBorrowingService.GetDeviceBorrowingHistory(username);

            // Kiểm tra nếu không có dữ liệu
            if (history == null || !history.Any())
            {
                return NotFound($"No borrowing history found for user '{username}'.");
            }

            // Trả về tất cả lịch sử mà không nhóm dữ liệu
            var response = history.Select(r => new
            {
                Id = r.Id,
                Username = r.Username,
                Description = r.Description,
                Status = r.Status,
                GroupStudents = r.GroupStudents.Select(g => new
                {
                    StudentName = g.StudentName,
                    LectureName = g.LectureName
                }).ToList(),
                DeviceBorrowingDetails = r.DeviceBorrowingDetails.Select(d => new
                {
                    DeviceId = d.DeviceId,
                    DeviceItemId = d.DeviceItemId,
                    Description = d.Description,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        // Delete a device borrowing request
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteDeviceBorrowingRequest(int id)
        {
            try
            {
                var result = await _deviceBorrowingService.DeleteDeviceBorrowingRequest(id);
                if (result)
                {
                    return Ok($"Device borrowing request with ID {id} deleted successfully.");
                }
                return NotFound($"Device borrowing request with ID {id} not found.");
            }
            catch (Exception ex)
            {
                // Log exception if necessary
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
