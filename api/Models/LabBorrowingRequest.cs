using System;
using System.Collections.Generic;

namespace api.Models
{
    public class LabBorrowingRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int LabId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
        public string Purpose { get; set; }
        public int LecturerInChargeId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }
        
        public List<DeviceBorrowingRequest> DeviceBorrowingRequests { get; set; }
    }
}
