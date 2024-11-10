using AutoMapper;
using api.Dtos.DeviceBorrowingRequest;
using api.Models;
using api.Dtos.Device;

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
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"))
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.DeviceIds));

            // Add the missing mapping here if required
            CreateMap<Device, UpdateDeviceRequestDto>()
                .ForMember(dest => dest.DeviceName, opt => opt.MapFrom(src => src.DeviceName))
                .ForMember(dest => dest.DeviceStatus, opt => opt.MapFrom(src => src.DeviceStatus));
        }
    }
}
