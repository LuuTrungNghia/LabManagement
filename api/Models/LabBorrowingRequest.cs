// using System.Collections.Generic;
// using api.Models;

// namespace api.Models
// {
//     public class LabBorrowingRequest
//     {
//         public int LabBorrowingRequestId { get; set; }

//         public int LabId { get; set; }

//         public Lab Lab { get; set; } // Quan hệ với Lab

//         public string UserId { get; set; }

//         public ApplicationUser User { get; set; } // Quan hệ với ApplicationUser

//         public ICollection<LabBorrowingDetail> LabBorrowingDetails { get; set; } = new List<LabBorrowingDetail>();
//     }
// }
// public class LabBorrowingDetail
//     {
//         public int LabBorrowingDetailId { get; set; }

//         public int LabBorrowingRequestId { get; set; }

//         public LabBorrowingRequest LabBorrowingRequest { get; set; } // Quan hệ với LabBorrowingRequest

//         public int DeviceId { get; set; }

//         public Device Device { get; set; } // Quan hệ với Device
//     }