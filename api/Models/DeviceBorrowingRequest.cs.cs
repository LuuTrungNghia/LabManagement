// using System;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.AspNetCore.Identity;

// namespace api.Models
// {
//     public class DeviceBorrowingRequest
//     {
//         public int DeviceBorrowingRequestId { get; set; }

//         [Required]
//         public int DeviceId { get; set; }

//         [ForeignKey("DeviceId")]
//         public Device Device { get; set; }

//         [Required]
//         public int RequestedQuantity { get; set; }

//         [Required]
//         public DateTime FromDate { get; set; }

//         [Required]
//         public DateTime ToDate { get; set; }

//         public DateTime? ApprovedDate { get; set; }

//         public DeviceItemStatus Status { get; set; } = DeviceItemStatus.Requested;

//         public string? ApprovedById { get; set; }

//         [ForeignKey("ApprovedById")]
//         public IdentityUser? ApprovedUser { get; set; }

//         public bool IsReturned { get; set; } = false;

//         public DateTime? ReturnDate { get; set; }
//     }
// }
