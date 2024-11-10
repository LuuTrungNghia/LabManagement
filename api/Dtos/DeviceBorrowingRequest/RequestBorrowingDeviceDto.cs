using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace api.Dtos.DeviceBorrowingRequest
{
    public class RequestBorrowingDeviceDto
    {
        [Required]
        public List<int> DeviceIds { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }
        
        public class BorrowDateValidationAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                DateTime borrowDate = (DateTime)value;
                return borrowDate > DateTime.Now; 
            }
        }
    }
}
