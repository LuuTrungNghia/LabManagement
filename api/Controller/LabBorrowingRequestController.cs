// using api.Models;
// using api.Services;
// using Microsoft.AspNetCore.Mvc;
// using System.Threading.Tasks;

// namespace api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class LabBorrowingRequestController : ControllerBase
//     {
//         private readonly ILabBorrowingRequestService _labBorrowingRequestService;

//         public LabBorrowingRequestController(ILabBorrowingRequestService labBorrowingRequestService)
//         {
//             _labBorrowingRequestService = labBorrowingRequestService;
//         }

//         // Tạo mới yêu cầu mượn phòng lab
//         [HttpPost]
//         public async Task<IActionResult> CreateLabBorrowingRequest([FromBody] LabBorrowingRequest labBorrowingRequest)
//         {
//             if (labBorrowingRequest == null)
//             {
//                 return BadRequest("Invalid request data.");
//             }

//             var createdRequest = await _labBorrowingRequestService.CreateLabBorrowingRequestAsync(labBorrowingRequest);
//             if (createdRequest == null)
//             {
//                 return NotFound("User or Lab not found.");
//             }

//             return Ok(createdRequest);
//         }

//         // Phê duyệt yêu cầu mượn phòng lab
//         [HttpPut("approve/{requestId}")]
//         public async Task<IActionResult> ApproveLabBorrowingRequest(int requestId)
//         {
//             var request = await _labBorrowingRequestService.ApproveLabBorrowingRequestAsync(requestId);
//             if (request == null)
//             {
//                 return NotFound("Lab Borrowing Request not found.");
//             }

//             return Ok(request);
//         }

//         // Lịch sử mượn phòng của người dùng
//         [HttpGet("history/{userId}")]
//         public async Task<IActionResult> GetLabBorrowingHistory(int userId)
//         {
//             var history = await _labBorrowingRequestService.GetLabBorrowingHistoryAsync(userId);
//             if (history == null)
//             {
//                 return NotFound("No borrowing history found.");
//             }

//             return Ok(history);
//         }

//         // Lấy thông tin yêu cầu mượn phòng cụ thể
//         [HttpGet("{requestId}")]
//         public async Task<IActionResult> GetLabBorrowingRequest(int requestId)
//         {
//             var request = await _labBorrowingRequestService.GetLabBorrowingRequestAsync(requestId);
//             if (request == null)
//             {
//                 return NotFound("Lab Borrowing Request not found.");
//             }

//             return Ok(request);
//         }
//     }
// }
