// using api.Dtos.Device;
// using api.Dtos.DeviceBorrowing;
// using api.Services;
// using Microsoft.AspNetCore.Mvc;

// namespace api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class DeviceBorrowingRequestsController : ControllerBase
//     {
//         private readonly DeviceBorrowingService _service;

//         public DeviceBorrowingRequestsController(DeviceBorrowingService service)
//         {
//             _service = service;
//         }

//         // Tạo yêu cầu mượn thiết bị
//         [HttpPost("create")]
//         public async Task<IActionResult> CreateRequest([FromBody] CreateDeviceBorrowingRequestDto dto)
//         {
//             await _service.CreateRequestAsync(dto);
//             return Ok();
//         }

//         // Phê duyệt yêu cầu mượn thiết bị
//         [HttpPut("approve/{id}")]
//         public async Task<IActionResult> ApproveRequest(int id, [FromQuery] string approvedById)
//         {
//             await _service.ApproveRequestAsync(id, approvedById);
//             return Ok();
//         }

//         // Xác nhận trả thiết bị
//         [HttpPut("confirm-return/{id}")]
//         public async Task<IActionResult> ConfirmReturn(int id)
//         {
//             await _service.ConfirmReturnAsync(id);
//             return Ok();
//         }

//         // Lấy lịch sử mượn thiết bị
//         [HttpGet("history")]
//         public async Task<IActionResult> GetHistory()
//         {
//             var history = await _service.GetHistoryAsync();
//             return Ok(history);
//         }
//     }
// }
