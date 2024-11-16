// using api.Models;
// using Microsoft.AspNetCore.Identity;

// public class DeviceBorrowingRequest
// {
//     public int Id { get; set; }
//     public string RequesterId { get; set; } // ID của sinh viên hoặc giảng viên
//     public string DeviceName { get; set; } // Tên thiết bị
//     public string DeviceType { get; set; } // Loại thiết bị
//     public int RequestedQuantity { get; set; } // Số lượng đăng ký
//     public int ActualBorrowedQuantity { get; set; } // Số lượng mượn thực tế
//     public DateTime FromDate { get; set; } // Thời gian bắt đầu mượn
//     public DateTime ToDate { get; set; } // Thời gian kết thúc mượn
//     public string Status { get; set; } = "Pending"; // Trạng thái (Pending, Approved, Returned)
//     public string ConditionOnReturn { get; set; } // Tình trạng thiết bị khi trả
//     public DateTime? ApprovalDate { get; set; } // Ngày phê duyệt
//     public DateTime? ReturnDate { get; set; } // Ngày hoàn trả

//     public virtual IdentityUser User { get; set; } // Sinh viên hoặc giảng viên
//     public virtual Device Device { get; set; }
// }
