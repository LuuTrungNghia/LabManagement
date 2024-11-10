using api.Dtos.DeviceBorrowingRequest;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceBorrowingRequestsController : ControllerBase
    {
        private readonly IDeviceBorrowingService _deviceBorrowingService;

        public DeviceBorrowingRequestsController(IDeviceBorrowingService deviceBorrowingService)
        {
            _deviceBorrowingService = deviceBorrowingService;
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowDevice([FromBody] RequestBorrowingDeviceDto dto)
        {
            var result = await _deviceBorrowingService.BorrowDeviceAsync(dto);
            
            if (!result.Success)
                return BadRequest(new { message = result.Message, code = "DEVICE_BORROW_FAILED" });

            return CreatedAtAction(nameof(GetRequestById), new { id = result.Id }, result.Data);
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateRequestStatus([FromBody] UpdateRequestStatusDto dto)
        {
            var result = await _deviceBorrowingService.UpdateRequestStatusAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Message, code = "STATUS_UPDATE_FAILED" });

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            var result = await _deviceBorrowingService.GetRequestByIdAsync(id);
            if (!result.Success)
                return NotFound(new { message = result.Message, code = "REQUEST_NOT_FOUND" });

            return Ok(result.Data);
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetBorrowingHistory(string userId)
        {
            var result = await _deviceBorrowingService.GetBorrowingHistoryAsync(userId);
            if (!result.Success)
                return NotFound(new { message = result.Message, code = "HISTORY_NOT_FOUND" });

            return Ok(result.Data);
        }
    }
}
