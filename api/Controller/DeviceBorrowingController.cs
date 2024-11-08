using api.Dtos.Device;
using api.Dtos.DeviceBorrowing;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/device-borrowings")]
    [ApiController]
    public class DeviceBorrowingController : ControllerBase
    {
        private readonly IDeviceBorrowingRepository _repo;

        public DeviceBorrowingController(IDeviceBorrowingRepository repo)
        {
            _repo = repo;
        }

        // Create a new device borrowing request
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateDeviceBorrowingRequestDto borrowingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var borrowing = new DeviceBorrowing
            {
                DeviceId = borrowingDto.DeviceId,
                StartDate = borrowingDto.StartDate,
                EndDate = borrowingDto.EndDate,
                BorrowerType = borrowingDto.BorrowerType,
                DeviceStatus = DeviceStatus.Borrowed,
                IsApproved = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = "System", // Replace with current logged in user
                UpdatedBy = "System"  // Replace with current logged in user
            };

            await _repo.CreateAsync(borrowing);
            return CreatedAtAction(nameof(GetById), new { id = borrowing.Id }, borrowing);
        }

        // Get a specific device borrowing request by ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var borrowing = await _repo.GetByIdAsync(id);
            if (borrowing == null)
                return NotFound();

            return Ok(borrowing);
        }

        // Update device borrowing request
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDeviceBorrowingRequestDto borrowingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var borrowing = await _repo.UpdateAsync(id, borrowingDto);
            if (borrowing == null)
                return NotFound();

            return Ok(borrowing);
        }

        // Approve a device borrowing request
        [HttpPut("approve/{id:int}")]
        public async Task<IActionResult> Approve(int id)
        {
            var borrowing = await _repo.ApproveAsync(id);
            if (borrowing == null)
                return NotFound();

            return Ok(borrowing);
        }

        // Mark the device as returned
        [HttpPut("return/{id:int}")]
        public async Task<IActionResult> ReturnDevice(int id, [FromBody] DeviceStatus status)
        {
            var borrowing = await _repo.ReturnAsync(id, status);
            if (borrowing == null)
                return NotFound();

            return Ok(borrowing);
        }

        // Get all borrowing requests by User ID
        [HttpGet("history/{userId:int}")]
        public async Task<IActionResult> GetHistory(int userId)
        {
            var borrowings = await _repo.GetByUserIdAsync(userId);
            if (!borrowings.Any())
                return NotFound();

            return Ok(borrowings);
        }
    }
}

