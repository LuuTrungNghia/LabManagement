using api.Dtos;
using api.Models;
using AutoMapper;

namespace api.MapperProfiles
{
    public class LabBorrowingMapper : Profile
    {
        public LabBorrowingMapper()
        {
            // Map from CreateLabBorrowingRequestDto to LabBorrowingRequest
            CreateMap<CreateLabBorrowingRequestDto, LabBorrowingRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => LabBorrowingStatus.Pending)) // Ensure the default status is Pending
                .ForMember(dest => dest.DeviceBorrowingDetails, opt => opt.MapFrom(src => src.DeviceBorrowingDetails)); // Map DeviceBorrowingDetails

            // Map from UpdateLabBorrowingRequestDto to LabBorrowingRequest
            CreateMap<UpdateLabBorrowingRequestDto, LabBorrowingRequest>()
                .ForMember(dest => dest.DeviceBorrowingDetails, opt => opt.MapFrom(src => src.DeviceBorrowingDetails)); // Map DeviceBorrowingDetails

            // Map from LabBorrowingRequest to LabBorrowingRequestDto
            CreateMap<LabBorrowingRequest, LabBorrowingRequestDto>()
                .ForMember(dest => dest.DeviceBorrowingDetails, opt => opt.MapFrom(src => src.DeviceBorrowingDetails));

            // Map from DeviceBorrowingDetailDto to DeviceBorrowingDetail
            CreateMap<DeviceBorrowingDetailDto, DeviceBorrowingDetail>();

            // Map from DeviceBorrowingRequestDto to DeviceBorrowingRequest
            CreateMap<DeviceBorrowingRequestDto, DeviceBorrowingRequest>()
                .ForMember(dest => dest.DeviceBorrowingDetails, opt => opt.MapFrom(src => src.DeviceBorrowingDetails));

            // Map from DeviceBorrowingRequest to DeviceBorrowingRequestDto
            CreateMap<DeviceBorrowingRequest, DeviceBorrowingRequestDto>()
                .ForMember(dest => dest.DeviceBorrowingDetails, opt => opt.MapFrom(src => src.DeviceBorrowingDetails)); 

            // Map from DeviceBorrowingDetail to DeviceBorrowingDetailDto
            CreateMap<DeviceBorrowingDetail, DeviceBorrowingDetailDto>();
        }
    }
}
