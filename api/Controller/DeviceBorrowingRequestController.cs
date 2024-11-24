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
        [Authorize(Roles = "admin, student, lecturer")]
        public async Task<IActionResult> GetDeviceBorrowingRequests()
        {
            var requests = await _deviceBorrowingService.GetDeviceBorrowingRequests();
            if (requests == null || !requests.Any())
            {
                return NotFound("No borrowing requests found.");
            }

            var response = requests
                .GroupBy(r => r.Username)
                .Select(group => new
                {
                    Id = group.First().Id,
                    Username = group.Key,
                    Description = group.First().Description,                    
                    Status = group.First().Status,
                    DeviceBorrowingDetails = group
                        .SelectMany(r => r.DeviceBorrowingDetails)
                        .GroupBy(d => new { d.DeviceId, d.DeviceItemId })
                        .Select(g => new
                        {
                            DeviceId = g.Key.DeviceId,
                            DeviceItemId = g.Key.DeviceItemId,
                            Description = string.Join(", ", g.Select(d => d.Description)),
                            StartDate = g.Min(d => d.StartDate),
                            EndDate = g.Max(d => d.EndDate), 
                        })
                        .ToList()
                })
                .ToList();

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
                // Validate student usernames
                if (requestDto.StudentUsernames != null && requestDto.StudentUsernames.Any())
                {
                    foreach (var studentUsername in requestDto.StudentUsernames)
                    {
                        var student = await _userManager.FindByNameAsync(studentUsername);
                        if (student == null)
                        {
                            return BadRequest($"Student with username '{studentUsername}' does not exist.");
                        }
                    }
                }

                // Validate lecturer username
                if (!string.IsNullOrEmpty(requestDto.LecturerUsername))
                {
                    var lecturer = await _userManager.FindByNameAsync(requestDto.LecturerUsername);
                    if (lecturer == null)
                    {
                        return BadRequest($"Lecturer with username '{requestDto.LecturerUsername}' does not exist.");
                    }
                }

                foreach (var detail in requestDto.DeviceBorrowingDetails)
                {
                    var existingRequest = await _deviceBorrowingService.CheckIfDeviceIsAvailable(detail.DeviceItemId);
                    if (existingRequest != null && existingRequest.Status != DeviceBorrowingStatus.Completed)
                    {
                        return BadRequest($"Device {detail.DeviceItemId} is already borrowed or not available.");
                    }
                }

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
                // Specific bad request error for invalid arguments
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // General server error
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
        [Authorize(Roles = "admin, student, lecturer")]
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
            if (!User.IsInRole("admin") && username != User.Identity.Name)
            {
                return Forbid("You are not authorized to view this history.");
            }

            var history = await _deviceBorrowingService.GetDeviceBorrowingHistory(username);
            var approvedHistory = history.Where(r => r.Status == DeviceBorrowingStatus.Approved).ToList();

            if (approvedHistory == null || !approvedHistory.Any())
            {
                return NotFound($"No approved borrowing history found for user '{username}'.");
            }

            var response = approvedHistory
                .GroupBy(r => r.Username)
                .Select(group => new
                {
                    Id = group.First().Id,
                    Username = group.Key,
                    Description = group.First().Description,                    
                    Status = group.First().Status,
                    DeviceBorrowingDetails = group
                        .SelectMany(r => r.DeviceBorrowingDetails)
                        .Select(d => new
                        {
                            DeviceId = d.DeviceId,
                            DeviceItemId = d.DeviceItemId,
                            Description = d.Description,
                            StartDate = d.StartDate,
                            EndDate = d.EndDate,
                        })
                        .Distinct()
                        .ToList()
                })
                .ToList();

            return Ok(response);
        }
    }
}
