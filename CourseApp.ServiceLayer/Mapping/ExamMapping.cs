using AutoMapper;
using CourseApp.EntityLayer.Dto.ExamDto;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.ServiceLayer.Mapping;

public class ExamMapping:Profile
{
    public ExamMapping()
    {
        CreateMap<Exam,GetAllExamDto>().ReverseMap();
        CreateMap<Exam,CreateExamDto>().ReverseMap();
        CreateMap<Exam,DeleteExamDto>().ReverseMap();
        CreateMap<Exam, MissingMappingDto>().ReverseMap();
    }
}
