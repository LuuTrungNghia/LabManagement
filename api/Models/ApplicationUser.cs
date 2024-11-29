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
        public string Avatar { get; set; } = "default-avatar.png";

        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        public bool IsApproved { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        public IEnumerable<DeviceBorrowingRequest> DeviceBorrowingRequests { get; set; } = new List<DeviceBorrowingRequest>();  // Change to IEnumerable
        public ICollection<LabBorrowingRequest> LabBorrowingRequests { get; set; } = new List<LabBorrowingRequest>();
    }
}
