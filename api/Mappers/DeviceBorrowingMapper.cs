using api.Dtos;
using api.Models;
using AutoMapper;

namespace api.MapperProfiles
{
    public class DeviceBorrowingMapper : Profile
    {
        public DeviceBorrowingMapper()
        {
            CreateMap<CreateDeviceBorrowingRequestDto, DeviceBorrowingRequest>();
            CreateMap<DeviceBorrowingRequest, DeviceBorrowingRequestDto>();
            CreateMap<UpdateDeviceBorrowingRequestDto, DeviceBorrowingRequest>();
            CreateMap<DeviceBorrowingDetailDto, DeviceBorrowingDetail>();
            CreateMap<DeviceBorrowingDetail, DeviceBorrowingDetailDto>();
        }
    }
}
