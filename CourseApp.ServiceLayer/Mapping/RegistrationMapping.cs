using AutoMapper;
using CourseApp.EntityLayer.Dto.RegistrationDto;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.ServiceLayer.Mapping;

public class RegistrationMapping:Profile
{
    public RegistrationMapping()
    {
        CreateMap<Registration,GetAllRegistrationDto>().ReverseMap();
        CreateMap<Registration,GetByIdRegistrationDto>().ReverseMap();
        CreateMap<Registration,CreateRegistrationDto>().ReverseMap();
        CreateMap<Registration,UpdatedRegistrationDto>().ReverseMap();
        CreateMap<Registration,DeleteRegistrationDto>().ReverseMap();
        CreateMap<Registration,GetAllRegistrationDetailDto>()
            .ForMember(dst => dst.CourseName,opt => opt.MapFrom(src => src.Course!.CourseName))
            .ForMember(dst => dst.StudentName,opt => opt.MapFrom(src => src.Student!.Name))
            .ReverseMap();
        CreateMap<Registration, GetByIdRegistrationDetailDto>()
            .ForMember(dst => dst.CourseName, opt => opt.MapFrom(src => src.Course!.CourseName))
            .ForMember(dst => dst.StudentName, opt => opt.MapFrom(SRC => SRC.Student!.Name))
            .ReverseMap();
        CreateMap<Registration, MissingRegistrationMappingDto>();
    }
}
