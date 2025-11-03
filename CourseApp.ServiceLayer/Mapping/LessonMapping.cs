using AutoMapper;
using CourseApp.EntityLayer.Dto.InstructorDto;
using CourseApp.EntityLayer.Dto.LessonDto;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.ServiceLayer.Mapping;

public class LessonMapping:Profile
{
    public LessonMapping()
    {
        CreateMap<Lesson, GetAllLessonDto>().ReverseMap();
        CreateMap<Lesson, CreateLessonDto>().ReverseMap();
        CreateMap<Lesson, DeleteLessonDto>().ReverseMap();
        CreateMap<Lesson, GetAllLessonDetailDto>()
                .ForMember(dst => dst.CourseName,opt => opt.MapFrom(src => src.Course!.CourseName))
                .ReverseMap();
        CreateMap<Lesson,GetByIdLessonDetailDto>()
                .ForMember(dst => dst.CourseName,opt => opt.MapFrom(src => src.Course!.CourseName))
                .ReverseMap();
        CreateMap<Lesson, NonExistentDtoType>();
    }
}
