using System;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.DeviceBorrowingRequest
{
    public class RequestBorrowingDeviceDto
    {
        [Required]
        public int DeviceId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }
    }
}
