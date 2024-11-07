using System;

namespace api.Models
{
    public class DeviceBorrowingRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DeviceId { get; set; }
        public int Quantity { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
        public string DeviceCondition { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }
        public int? LabBorrowingRequestId { get; set; } 
        public int? RoomBookingRequestId { get; set; }
    }
}
