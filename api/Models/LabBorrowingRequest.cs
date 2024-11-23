// using System;
// using System.Collections.Generic;

// namespace api.Models
// {
//     public class LabBorrowingRequest
//     {
//         public int LabBorrowingRequestId { get; set; } // ID của yêu cầu mượn phòng lab
//         public int UserId { get; set; } // ID người yêu cầu
//         public ApplicationUser User { get; set; } // Người dùng yêu cầu mượn phòng
//         public int LabId { get; set; } // ID phòng lab
//         public Lab Lab { get; set; } // Phòng lab được mượn
//         public DateTime StartDate { get; set; } // Ngày bắt đầu mượn phòng
//         public DateTime EndDate { get; set; } // Ngày kết thúc mượn phòng
//         public bool IsApproved { get; set; } // Trạng thái phê duyệt (được/không)
//         public ICollection<DeviceBorrowingRequest> DeviceBorrowingRequests { get; set; } // Các thiết bị mượn kèm
//     }
// }
