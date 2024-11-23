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
            return Ok(requests);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin, student, lecturer")]
        public async Task<IActionResult> GetDeviceBorrowingRequest(int id)
        {
            var request = await _deviceBorrowingService.GetDeviceBorrowingRequestById(id);
            if (request == null || (User.IsInRole("active") && request.Username != User.Identity.Name))
            {
                return NotFound();
            }
            return Ok(request);
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin, student, lecturer")]
        public async Task<IActionResult> CreateDeviceBorrowingRequest([FromBody] CreateDeviceBorrowingRequestDto requestDto)
        {
            var result = await _deviceBorrowingService.CreateDeviceBorrowingRequest(requestDto);
            if (result == null)
            {
                return BadRequest("Could not create borrowing request.");
            }
            return Ok(result);
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
            // Admin can view any user's history, while others can only view their own
            if (!User.IsInRole("admin") && username != User.Identity.Name)
            {
                return Forbid("You are not authorized to view this history.");
            }

            var history = await _deviceBorrowingService.GetDeviceBorrowingHistory(username);

            if (history == null || !history.Any())
            {
                return NotFound($"No borrowing history found for user '{username}'.");
            }

            return Ok(history);
        }
    }
}
