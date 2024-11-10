using AutoMapper;
using api.Dtos.DeviceBorrowingRequest;
using api.Models;

namespace api.Mappers
{
    public class DeviceBorrowingRequestMapper : Profile
    {
        public DeviceBorrowingRequestMapper()
        {
            CreateMap<DeviceBorrowingRequest, DeviceBorrowingRequestDto>()
                .ForMember(dest => dest.DeviceName, opt => opt.MapFrom(src => src.Device.DeviceName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FullName.UserName));
           
            CreateMap<RequestBorrowingDeviceDto, DeviceBorrowingRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"));
        }
    }
}
