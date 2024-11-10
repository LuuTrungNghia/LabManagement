using System;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class DeviceBorrowingRequest
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string UserId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; }

        public Device Device { get; set; }
        public IdentityUser User { get; set; }
    }
}
