using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Dtos;
using api.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceBorrowingController : ControllerBase
    {
        private readonly IDeviceBorrowingService _deviceBorrowingService;

        public DeviceBorrowingController(IDeviceBorrowingService deviceBorrowingService)
        {
            _deviceBorrowingService = deviceBorrowingService;
        }

        [HttpGet]
        [Authorize(Roles = "admin, student, lecturer")]
        public async Task<IActionResult> GetDeviceBorrowingRequests()
        {
            var requests = await _deviceBorrowingService.GetDeviceBorrowingRequests();
            if (requests == null || !requests.Any())
            {
                return NotFound(); // Ensure 404 is returned if no request is found.
            }

            // Group requests by username and merge the device details
            var response = requests
                .GroupBy(r => r.Username) // Group by Username or UserId
                .Select(group => new
                {
                    Id = group.First().Id, // Use the first request's ID for the grouped entry
                    Username = group.Key,
                    Description = group.First().Description, // Assuming all requests in a group have the same description
                    FromDate = group.Min(r => r.FromDate), // Assuming the FromDate should be the same for grouped requests
                    ToDate = group.Max(r => r.ToDate), // Same for ToDate
                    Status = group.First().Status, // Assuming status is the same for all requests in the group
                    DeviceBorrowingDetails = group
                        .SelectMany(r => r.DeviceBorrowingDetails) // Flatten all the device borrowing details for the group
                        .GroupBy(d => new { d.DeviceId, d.DeviceItemId }) // Group by DeviceId and DeviceItemId
                        .Select(g => new
                        {
                            DeviceId = g.Key.DeviceId,
                            DeviceItemId = g.Key.DeviceItemId,
                            Description = string.Join(", ", g.Select(d => d.Description)) // Combine descriptions if needed
                        })
                        .ToList()
                })
                .ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin, student, lecturer")]
        public async Task<IActionResult> GetDeviceBorrowingRequest(int id)
        {
            var request = await _deviceBorrowingService.GetDeviceBorrowingRequestById(id);
            if (request == null || (User.IsInRole("student") && request.Username != User.Identity.Name))
            {
                return NotFound();
            }

            // Create the response structure as per the new format
            var response = new 
            {
                Id = request.Id,
                Username = request.Username,
                Description = request.Description,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Status = request.Status,
                DeviceBorrowingDetails = request.DeviceBorrowingDetails
                    .GroupBy(d => new { d.DeviceId, d.DeviceItemId }) // Group by DeviceId and DeviceItemId
                    .Select(group => new 
                    {
                        DeviceId = group.Key.DeviceId,
                        DeviceItemId = group.Key.DeviceItemId,
                        Description = string.Join(", ", group.Select(d => d.Description)) // Combine descriptions if needed
                    })
                    .ToList()
            };

            return Ok(response);
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin, student, lecturer")]
        public async Task<IActionResult> CreateDeviceBorrowingRequest([FromBody] CreateDeviceBorrowingRequestDto requestDto)
        {
            try
            {
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

        [HttpGet("history/{username}")]
        [Authorize(Roles = "admin, student, lecturer")]
        public async Task<IActionResult> GetDeviceBorrowingHistory(string username)
        {
            if (!User.IsInRole("admin") && username != User.Identity.Name)
            {
                return Forbid("You are not authorized to view this history.");
            }

            // Fetch the approved history
            var history = await _deviceBorrowingService.GetDeviceBorrowingHistory(username);
            var approvedHistory = history.Where(r => r.Status == DeviceBorrowingStatus.Approved).ToList();

            if (approvedHistory == null || !approvedHistory.Any())
            {
                return NotFound($"No approved borrowing history found for user '{username}'.");
            }

            // Group requests by username and merge the device details
            var response = approvedHistory
                .GroupBy(r => r.Username)
                .Select(group => new
                {
                    Id = group.First().Id, // Use the first request's ID for the grouped entry
                    Username = group.Key,
                    Description = group.First().Description,
                    FromDate = group.Min(r => r.FromDate), // Assuming the FromDate should be the same for grouped requests
                    ToDate = group.Max(r => r.ToDate), // Same for ToDate
                    Status = group.First().Status, // Assuming status is the same for all requests in the group
                    DeviceBorrowingDetails = group
                        .SelectMany(r => r.DeviceBorrowingDetails) // Flatten all the device borrowing details for the group
                        .Select(d => new 
                        {
                            DeviceId = d.DeviceId,
                            DeviceItemId = d.DeviceItemId,
                            Description = d.Description
                        })
                        .Distinct() // Ensure unique devices
                        .ToList()
                })
                .ToList();

            return Ok(response);
        }
    }
}
