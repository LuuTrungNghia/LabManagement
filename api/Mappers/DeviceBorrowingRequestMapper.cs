// using api.Dtos.DeviceBorrowing;
// using api.Models;

// namespace api.Mappers
// {
//     public static class DeviceBorrowingRequestMapper
//     {
//         public static DeviceBorrowingRequestDto ToDto(this DeviceBorrowingRequest request)
//         {
//             return new DeviceBorrowingRequestDto
//             {
//                 DeviceBorrowingRequestId = request.DeviceBorrowingRequestId,
//                 DeviceId = request.DeviceId,
//                 DeviceName = request.Device.DeviceName,
//                 RequestedQuantity = request.RequestedQuantity,
//                 FromDate = request.FromDate,
//                 ToDate = request.ToDate,
//                 ApprovedDate = request.ApprovedDate,
//                 Status = request.Status,
//                 IsReturned = request.IsReturned,
//                 ReturnDate = request.ReturnDate
//             };
//         }
//     }
// }
