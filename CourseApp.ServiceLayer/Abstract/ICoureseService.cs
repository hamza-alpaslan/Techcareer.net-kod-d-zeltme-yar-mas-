using CourseApp.EntityLayer.Dto.CourseDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Utilities.Result;
using System.Linq.Expressions;

namespace CourseApp.ServiceLayer.Abstract;

public interface ICourseService
{
    Task<IDataResult<IEnumerable<GetAllCourseDto>>> GetAllAsync(bool track = true);
    Task<IDataResult<GetByIdCourseDto>> GetByIdAsync(string id, bool track = true);
    Task<IResult> CreateAsync(CreateCourseDto entity);
    Task<IResult> Update(UpdateCourseDto entity);
    Task<IResult> Remove(DeleteCourseDto entity);
    Task<IDataResult<IEnumerable<GetAllCourseDetailDto>>> GetAllCourseDetail(bool track = true);
}



