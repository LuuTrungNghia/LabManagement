using api.Models;
using api.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace api.Mappers
{
    public static class LabBorrowingMapper
    {
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
                }).ToList(),
                DeviceBorrowingDetails = model.DeviceBorrowingDetails?.Select(dbd => new DeviceBorrowingDetailDto
                {
                    DeviceId = dbd.DeviceId,
                    DeviceItemId = dbd.DeviceItemId,
                    Description = dbd.Description,
                    StartDate = dbd.StartDate,
                    EndDate = dbd.EndDate
                }).ToList()
            };
        }

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
                DeviceBorrowingDetails = dto.DeviceBorrowingDetails?.Select(dbd => new DeviceBorrowingDetail
                {
                    DeviceId = dbd.DeviceId,
                    DeviceItemId = dbd.DeviceItemId,
                    Description = dbd.Description,
                    StartDate = dbd.StartDate,
                    EndDate = dbd.EndDate
                }).ToList()
            };
        }

        public static LabBorrowingRequest ToModel(UpdateLabBorrowingRequestDto dto)
        {
            return new LabBorrowingRequest
            {
                Description = dto.Description,
                DeviceBorrowingDetails = dto.DeviceBorrowingDetails?.Select(dbd => new DeviceBorrowingDetail
                {
                    DeviceId = dbd.DeviceId,
                    DeviceItemId = dbd.DeviceItemId,
                    Description = dbd.Description,
                    StartDate = dbd.StartDate,
                    EndDate = dbd.EndDate
                }).ToList()
            };
        }
    }
}

