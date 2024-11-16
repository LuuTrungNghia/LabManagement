// using api.Dtos.Device;
// using api.Dtos;
// using api.Services;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Authorization;
// using api.Interfaces;

// namespace LabManagementSystem.Controllers
// {
//     [ApiController]
// [Route("api/[controller]")]
// public class DeviceBorrowingRequestsController : ControllerBase
// {
//     private readonly IDeviceBorrowingRequestRepository _deviceBorrowingRequestRepository;
//     private readonly IMapper _mapper;

//     public DeviceBorrowingRequestsController(IDeviceBorrowingRequestRepository deviceBorrowingRequestRepository, IMapper mapper)
//     {
//         _deviceBorrowingRequestRepository = deviceBorrowingRequestRepository;
//         _mapper = mapper;
//     }

//     // Tạo mới đơn đăng ký cho sinh viên hoặc giảng viên
//     [HttpPost("create")]
//     public async Task<IActionResult> CreateRequest(CreateDeviceBorrowingRequestDto requestDto)
//     {
//         var request = _mapper.Map<DeviceBorrowingRequest>(requestDto);
//         request.Status = "Pending";
        
//         await _deviceBorrowingRequestRepository.CreateRequestAsync(request);
//         return Ok("Device borrowing request created successfully.");
//     }

//     // Phê duyệt đơn đăng ký
//     [HttpPut("approve/{id}")]
//     public async Task<IActionResult> ApproveRequest(int id)
//     {
//         await _deviceBorrowingRequestRepository.ApproveRequestAsync(id);
//         return Ok("Device borrowing request approved.");
//     }

//     // Lịch sử mượn/hoàn trả
//     [HttpGet("history/{requesterId}")]
//     public async Task<IActionResult> GetRequestHistory(string requesterId)
//     {
//         var history = await _deviceBorrowingRequestRepository.GetRequestHistoryAsync(requesterId);
//         var historyDto = _mapper.Map<IEnumerable<DeviceBorrowingRequestDto>>(history);
//         return Ok(historyDto);
//     }

//     // Xác nhận trả thiết bị
//     [HttpPut("confirm-return/{id}")]
//     public async Task<IActionResult> ConfirmReturn(int id, [FromBody] UpdateDeviceBorrowingRequestDto updateDto)
//     {
//         await _deviceBorrowingRequestRepository.ConfirmReturnAsync(id, updateDto.ConditionOnReturn);
//         return Ok("Device return confirmed.");
//     }
// }

// }