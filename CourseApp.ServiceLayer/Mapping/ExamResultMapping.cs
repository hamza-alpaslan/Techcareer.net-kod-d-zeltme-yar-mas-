using AutoMapper;
using CourseApp.EntityLayer.Dto.ExamResultDto;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.ServiceLayer.Mapping;

public class ExamResultMapping:Profile
{
    public ExamResultMapping()
    {
        CreateMap<ExamResult,GetAllExamResultDto>().ReverseMap();
        CreateMap<ExamResult,CreateExamResultDto>().ReverseMap();
        CreateMap<ExamResult, DeleteExamResultDto>().ReverseMap();
        CreateMap<ExamResult, GetAllExamResultDetailDto>()
            .ForMember(dst => dst.StudentName,opt => opt.MapFrom(src => src.Student!.Name))
            .ForMember(dst => dst.StudentSurname,opt => opt.MapFrom(src => src.Student!.Surname))
            .ForMember(dst => dst.ExamName,opt => opt.MapFrom(src => src.Exam!.Name))
            .ReverseMap();
       CreateMap<ExamResult,GetByIdExamResultDetailDto>()
            .ForMember(dst => dst.StudentName, opt => opt.MapFrom(src => src.Student!.Name))
            .ForMember(dst => dst.StudentSurname, opt => opt.MapFrom(src => src.Student!.Surname))
            .ForMember(dst => dst.ExamName, opt => opt.MapFrom(src => src.Exam!.Name))
            .ReverseMap();
        CreateMap<ExamResult, MissingMappingClass>();
    }
}
