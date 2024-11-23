// using api.Models;
// using api.Repositories;

// public static class LabBorrowingMapper
// {
//     // Maps LabBorrowingRequest to LabBorrowingRequestDto
//     public static LabBorrowingRequestDto ToDto(this LabBorrowingRequest request)
//     {
//         return new LabBorrowingRequestDto
//         {
//             Username = request.User.UserName, // Assuming UserName is from ApplicationUser
//             LabId = request.LabId,
//             StartDate = request.StartDate,
//             EndDate = request.EndDate,
//             DeviceBorrowingRequests = request.DeviceBorrowingRequests.Select(d => d.ToDto()).ToList()
//         };
//     }

//     // Maps CreateLabBorrowingRequestDto to LabBorrowingRequest
//     public static LabBorrowingRequest ToEntity(this CreateLabBorrowingRequestDto dto, ApplicationUser user, Lab lab)
//     {
//         return new LabBorrowingRequest
//         {
//             UserId = user.Id,
//             LabId = lab.LabId,
//             StartDate = dto.FromDate,
//             EndDate = dto.ToDate,
//             DeviceBorrowingRequests = dto.DeviceIds.Select(d => new DeviceBorrowingRequest
//             {
//                 DeviceId = d,
//                 LabBorrowingRequestId = lab.LabBorrowingRequestId
//             }).ToList()
//         };
//     }
// }
