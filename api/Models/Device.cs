using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Device
    {
       public int Id { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }  // Loại thiết bị (ví dụ: Máy tính, Máy chiếu)
        public int Quantity { get; set; }  // Số lượng thiết bị
        public string DeviceStatus { get; set; }  // Tình trạng thiết bị (Tốt, Hư hỏng)
        public bool IsAvailable { get; set; }  // Thiết bị còn sử dụng được không
        public string Manufacturer { get; set; }  // Nhà sản xuất thiết bị
        public DateTime DatePurchased { get; set; }  // Ngày mua thiết bị
        public string DeviceModel { get; set; }  // Mẫu thiết bị
        public string SerialNumber { get; set; } // Số seri thiết bị
         public List<DeviceBorrowingRequest> BorrowingRequests { get; set; }  // Danh sách yêu cầu mượn thiết bị
    }
}

