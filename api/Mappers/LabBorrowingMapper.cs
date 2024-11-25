using api.Models;
using api.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace api.Mappers
{
    public static class LabBorrowingMapper
    {
        // Chuyển đổi từ LabBorrowingRequest thành LabBorrowingRequestDto
        public static LabBorrowingRequestDto ToDto(LabBorrowingRequest model)
        {
            return new LabBorrowingRequestDto
            {
                Id = model.Id,
                Username = model.Username,
                Description = model.Description,
                Status = model.Status,
                GroupStudents = model.GroupStudents?.Select(gs => new GroupStudentDto
                {
                    StudentName = gs.StudentName,
                    LectureName = gs.LectureName
                }).ToList() ?? new List<GroupStudentDto>(),

                // Kiểm tra mượn thiết bị và thêm DeviceBorrowingDetails chỉ khi cần thiết
                DeviceBorrowingRequests = model.DeviceBorrowingRequests.Select(d => new DeviceBorrowingRequestDto
                {
                    Id = d.Id,
                    Username = d.Username,
                    Description = d.Description,
                    Status = d.Status,
                    // Nếu là mượn thiết bị thì thêm DeviceBorrowingDetails
                    DeviceBorrowingDetails = d.DeviceBorrowingDetails != null
                        ? d.DeviceBorrowingDetails.Select(dd => new DeviceBorrowingDetailDto
                        {
                            DeviceId = dd.DeviceId,
                            DeviceItemId = dd.DeviceItemId,
                            Description = dd.Description,
                            StartDate = dd.StartDate,
                            EndDate = dd.EndDate
                        }).ToList()
                        : null // Nếu không phải mượn thiết bị, trả về null hoặc không thêm gì
                }).ToList()
            };
        }

        // Chuyển đổi từ CreateLabBorrowingRequestDto thành LabBorrowingRequest
        public static LabBorrowingRequest ToModel(CreateLabBorrowingRequestDto dto)
        {
            return new LabBorrowingRequest
            {
                Username = dto.Username,
                Description = dto.Description,
                Status = LabBorrowingStatus.Pending,
                GroupStudents = dto.GroupStudents?.Select(gs => new GroupStudent
                {
                    StudentName = gs.StudentName,
                    LectureName = gs.LectureName
                }).ToList(),
                
                // Chỉ thêm DeviceBorrowingRequests, không cần DeviceBorrowingDetails cho mượn phòng lab
                DeviceBorrowingRequests = dto.DeviceBorrowingRequests.Select(d => new DeviceBorrowingRequest
                {
                    Username = dto.Username,
                    Description = d.Description,
                    Status = DeviceBorrowingStatus.Pending,
                    // Không cần thêm DeviceBorrowingDetails cho mượn phòng lab
                    DeviceBorrowingDetails = new List<DeviceBorrowingDetail>()
                }).ToList()
            };
        }

        // Chuyển đổi từ UpdateLabBorrowingRequestDto thành LabBorrowingRequest
        public static LabBorrowingRequest ToModel(UpdateLabBorrowingRequestDto dto)
        {
            return new LabBorrowingRequest
            {
                Description = dto.Description,
                DeviceBorrowingRequests = dto.DeviceBorrowingRequests.Select(d => new DeviceBorrowingRequest
                {
                    Id = d.Id,
                    Description = d.Description,
                    Status = d.Status,
                    // Không thêm DeviceBorrowingDetails cho mượn phòng lab
                    DeviceBorrowingDetails = new List<DeviceBorrowingDetail>()
                }).ToList()
            };
        }
    }
}
