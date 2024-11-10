using api.Dtos.DeviceBorrowing;
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

        // Tạo yêu cầu mượn thiết bị
        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowDevice([FromBody] RequestBorrowingDeviceDto dto)
        {
            var result = await _deviceBorrowingService.BorrowDeviceAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        // Cập nhật trạng thái đơn mượn
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateRequestStatus([FromBody] UpdateRequestStatusDto dto)
        {
            var result = await _deviceBorrowingService.UpdateRequestStatusAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        // Lấy yêu cầu mượn theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            var result = await _deviceBorrowingService.GetRequestByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        // Lấy tất cả yêu cầu mượn thiết bị
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRequests()
        {
            var result = await _deviceBorrowingService.GetAllRequestsAsync();
            return Ok(result.Data);
        }

        // Lịch sử mượn thiết bị của người dùng
        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetBorrowingHistory(string userId)
        {
            var result = await _deviceBorrowingService.GetBorrowingHistoryAsync(userId);
            return Ok(result.Data);
        }
    }
}
