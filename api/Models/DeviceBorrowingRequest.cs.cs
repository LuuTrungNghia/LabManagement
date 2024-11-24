using System.ComponentModel.DataAnnotations;
using api.Models;

public class DeviceBorrowingRequest
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        public DeviceBorrowingStatus Status { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<DeviceBorrowingDetail> DeviceBorrowingDetails { get; set; } = new List<DeviceBorrowingDetail>();
        public List<string> StudentUsernames { get; set; } = new List<string>(); 
        public string LecturerUsername { get; set; }
        
        // Foreign key for LabBorrowingRequest
        public int? LabBorrowingRequestId { get; set; }
        public LabBorrowingRequest LabBorrowingRequest { get; set; }

        // Foreign key for Device
        public int? DeviceId { get; set; }
        public Device Device { get; set; }
    }

    public class DeviceBorrowingDetail
    {
        public int Id { get; set; }

        // Foreign key for DeviceBorrowingRequest
        public int DeviceBorrowingRequestId { get; set; }
        public DeviceBorrowingRequest DeviceBorrowingRequest { get; set; }

        // Foreign key for Device
        public int DeviceId { get; set; }
        public Device Device { get; set; }

        // Foreign key for DeviceItem
        public int DeviceItemId { get; set; }
        public DeviceItem DeviceItem { get; set; }

        public string Description { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public enum DeviceBorrowingStatus
    {
        Pending,
        Approved,
        Rejected,
        Completed
    }
