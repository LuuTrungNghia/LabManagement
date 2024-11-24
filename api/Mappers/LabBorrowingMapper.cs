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
                DeviceBorrowingRequests = model.DeviceBorrowingRequests.Select(d => new DeviceBorrowingRequestDto
                {
                    Id = d.Id,
                    Username = d.Username,
                    Description = d.Description,                   
                    Status = d.Status,
                    DeviceBorrowingDetails = d.DeviceBorrowingDetails.Select(dd => new DeviceBorrowingDetailDto
                    {
                        DeviceId = dd.DeviceId,
                        DeviceItemId = dd.DeviceItemId,
                        Description = dd.Description,
                        StartDate = dd.StartDate,
                        EndDate = dd.EndDate,
                    }).ToList()
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
                DeviceBorrowingRequests = dto.DeviceBorrowingRequests.Select(d => new DeviceBorrowingRequest
                {
                    Username = dto.Username,
                    Description = d.Description,
                    Status = DeviceBorrowingStatus.Pending,
                    DeviceBorrowingDetails = new List<DeviceBorrowingDetail>()
                }).ToList()
            };
        }

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
                    DeviceBorrowingDetails = new List<DeviceBorrowingDetail>()
                }).ToList()
            };
        }
    }
}
