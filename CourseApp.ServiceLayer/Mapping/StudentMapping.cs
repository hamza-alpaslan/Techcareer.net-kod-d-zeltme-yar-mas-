using AutoMapper;
using CourseApp.EntityLayer.Dto.RegistrationDto;
using CourseApp.EntityLayer.Dto.StudentDto;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.ServiceLayer.Mapping;

public class StudentMapping:Profile
{
    public StudentMapping()
    {
        CreateMap<Student, GetAllStudentDto>().ReverseMap();
        CreateMap<Student, GetByIdStudentDto>().ReverseMap();
        CreateMap<Student, CreateStudentDto>().ReverseMap();
        CreateMap<Student, UpdateStudentDto>().ReverseMap();
        CreateMap<Student, DeleteStudentDto>().ReverseMap();
        CreateMap<Student, NonExistentStudentMappingDto>().ReverseMap();
    }
}
