using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dtos;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabBorrowingRequestsController : ControllerBase
    {
        private readonly ILabBorrowingService _service;

        public LabBorrowingRequestsController(ILabBorrowingService service)
        {
            _service = service;
        }

        // Get all Lab Borrowing Requests
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<LabBorrowingRequestDto>>> GetAllLabBorrowingRequests()
        {
            var requests = await _service.GetAllLabBorrowingRequestsAsync();
            return Ok(requests);
        }

        // Get Lab Borrowing Request by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<LabBorrowingRequestDto>> GetLabBorrowingRequestById(int id)
        {
            var request = await _service.GetLabBorrowingRequestByIdAsync(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        // Create Lab Borrowing Request
        [HttpPost]
        [Authorize(Roles = "admin,lecturer,student")]
        public async Task<ActionResult<LabBorrowingRequestDto>> CreateLabBorrowingRequest(CreateLabBorrowingRequestDto dto)
        {
            var createdRequest = await _service.CreateLabBorrowingRequestAsync(dto);
            if (createdRequest == null) return BadRequest();
            return CreatedAtAction(nameof(GetLabBorrowingRequestById), new { id = createdRequest.Id }, createdRequest);
        }

        // Update Lab Borrowing Request
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<LabBorrowingRequestDto>> UpdateLabBorrowingRequest(int id, UpdateLabBorrowingRequestDto dto)
        {
            var updatedRequest = await _service.UpdateLabBorrowingRequestAsync(id, dto);
            if (updatedRequest == null) return NotFound();
            return Ok(updatedRequest);
        }

        // Delete Lab Borrowing Request
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteLabBorrowingRequest(int id)
        {
            var success = await _service.DeleteLabBorrowingRequestAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
        
        // Approve Lab Borrowing Request
        [HttpPost("approve/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<LabBorrowingRequestDto>> ApproveLabBorrowingRequest(int id)
        {
            var updatedRequest = await _service.ApproveLabBorrowingRequestAsync(id);
            if (updatedRequest == null) return NotFound();
            return Ok(updatedRequest);
        }

        // Reject Lab Borrowing Request
        [HttpPost("reject/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<LabBorrowingRequestDto>> RejectLabBorrowingRequest(int id)
        {
            var updatedRequest = await _service.RejectLabBorrowingRequestAsync(id);
            if (updatedRequest == null) return NotFound();
            return Ok(updatedRequest);
        }

        [HttpGet("history/{username}")]
        [Authorize(Roles = "admin, student, lecturer")]
        public async Task<IActionResult> GetLabBorrowingHistory(string username)
        {
            // Ensure the user has permission to view the history
            if (!User.IsInRole("admin") && username != User.Identity.Name)
            {
                return Forbid("You are not authorized to view this history.");
            }

            // Retrieve the lab borrowing history
            var history = await _service.GetLabBorrowingHistoryAsync(username);

            // If no history found, return a NotFound response
            if (history == null || !history.Any())
            {
                return NotFound($"No borrowing history found for user '{username}'.");
            }

            // Map the result into DTO for response
            var response = history.Select(r => new LabBorrowingRequestHistoryDto
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

            return Ok(response);
        }
    }
}
