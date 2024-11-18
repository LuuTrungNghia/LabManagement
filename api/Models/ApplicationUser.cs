using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [Url]
        public string Avatar { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        public ICollection<DeviceBorrowingRequest> DeviceBorrowingRequests { get; set; } = new List<DeviceBorrowingRequest>();

        public ICollection<LabBorrowingRequest> LabBorrowingRequests { get; set; } = new List<LabBorrowingRequest>();
    }
}
