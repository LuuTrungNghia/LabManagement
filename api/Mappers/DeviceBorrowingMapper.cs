using api.Dtos;
using api.Models;
using AutoMapper;

namespace api.MapperProfiles
{
    public class DeviceBorrowingMapper : Profile
    {
        public DeviceBorrowingMapper()
        {
            // Map from CreateDeviceBorrowingRequestDto to DeviceBorrowingRequest
            CreateMap<CreateDeviceBorrowingRequestDto, DeviceBorrowingRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => DeviceBorrowingStatus.Pending)); // Default status

            // Map from DeviceBorrowingRequest to DeviceBorrowingRequestDto
            CreateMap<DeviceBorrowingRequest, DeviceBorrowingRequestDto>()
                .ForMember(dest => dest.DeviceBorrowingDetails, opt => opt.MapFrom(src => src.DeviceBorrowingDetails))
                .ForMember(dest => dest.GroupStudents, opt => opt.MapFrom(src => src.GroupStudents));

            // Map from UpdateDeviceBorrowingRequestDto to DeviceBorrowingRequest
            CreateMap<UpdateDeviceBorrowingRequestDto, DeviceBorrowingRequest>()
                .ForMember(dest => dest.DeviceBorrowingDetails, opt => opt.MapFrom(src => src.DeviceBorrowingDetails))
                .ForMember(dest => dest.GroupStudents, opt => opt.MapFrom(src => src.GroupStudents));

            // Map from DeviceBorrowingDetailDto to DeviceBorrowingDetail
            CreateMap<DeviceBorrowingDetailDto, DeviceBorrowingDetail>();

            // Map from DeviceBorrowingDetail to DeviceBorrowingDetailDto
            CreateMap<DeviceBorrowingDetail, DeviceBorrowingDetailDto>();

            // Additional mappings if needed
        }
    }
}
