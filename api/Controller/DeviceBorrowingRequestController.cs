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

        // Thêm phân quyền cho phép admin và active người dùng xem tất cả yêu cầu mượn thiết bị
        [HttpGet]
        [Authorize(Roles = "admin, active")]
        public async Task<IActionResult> GetDeviceBorrowingRequests()
        {
            var requests = await _deviceBorrowingService.GetDeviceBorrowingRequests();
            return Ok(requests);
        }

        // Lấy yêu cầu mượn thiết bị theo ID, chỉ cho phép người dùng truy cập yêu cầu của chính họ hoặc admin
        [HttpGet("{id}")]
        [Authorize(Roles = "admin, active")]
        public async Task<IActionResult> GetDeviceBorrowingRequest(int id)
        {
            // Kiểm tra xem người dùng có phải là người tạo yêu cầu không, hoặc nếu là admin thì cho phép
            var request = await _deviceBorrowingService.GetDeviceBorrowingRequestById(id);
            if (request == null || (User.IsInRole("active") && request.Username != User.Identity.Name))
            {
                return NotFound();
            }
            return Ok(request);
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin, active")]
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

        // Phê duyệt yêu cầu mượn thiết bị, chỉ cho phép admin
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

        // Trả lại thiết bị, cho phép người dùng active và admin
        [HttpPost("return")]
        [Authorize(Roles = "admin, active")]
        public async Task<IActionResult> ReturnDevice([FromBody] DeviceReturnDto deviceReturnDto)
        {
            var result = await _deviceBorrowingService.ReturnDevice(deviceReturnDto);
            if (result)
            {
                return Ok("Device returned successfully.");
            }
            return BadRequest("Could not return device.");
        }

        // Lịch sử mượn thiết bị, chỉ cho phép admin xem lịch sử của người khác
        [HttpGet("history/{username}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetDeviceBorrowingHistory(string username)
        {
            var history = await _deviceBorrowingService.GetDeviceBorrowingHistory(username);
            return Ok(history);
        }
    }
}
