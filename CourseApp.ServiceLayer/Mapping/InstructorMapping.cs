using AutoMapper;
using CourseApp.EntityLayer.Dto.InstructorDto;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.ServiceLayer.Mapping;

public class InstructorMapping:Profile
{
    public InstructorMapping()
    {
        CreateMap<Instructor,GetAllInstructorDto>().ReverseMap();
        CreateMap<Instructor,GetByIdInstructorDto>().ReverseMap();
        CreateMap<Instructor,DeletedInstructorDto>().ReverseMap();
        CreateMap<Instructor, UndefinedMappingDto>();
    }
}
