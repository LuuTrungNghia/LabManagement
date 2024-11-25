using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map từ Lab sang LabDto và ngược lại
        CreateMap<Lab, LabDto>().ReverseMap();
    }
}
