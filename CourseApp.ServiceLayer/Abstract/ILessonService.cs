using CourseApp.EntityLayer.Dto.ExamResultDto;
using CourseApp.EntityLayer.Dto.LessonDto;
using CourseApp.ServiceLayer.Utilities.Result;

namespace CourseApp.ServiceLayer.Abstract;

public interface ILessonService
{
    Task<IDataResult<IEnumerable<GetAllLessonDto>>> GetAllAsync(bool track = true);
    Task<IDataResult<GetByIdLessonDto>> GetByIdAsync(string id, bool track = true);
    Task<IResult> CreateAsync(CreateLessonDto entity);
    Task<IResult> Update(UpdateLessonDto entity);
    Task<IResult> Remove(DeleteLessonDto entity);
    Task<IDataResult<IEnumerable<GetAllLessonDetailDto>>> GetAllLessonDetailAsync(bool track = true);
    Task<IDataResult<GetByIdLessonDetailDto>> GetByIdLessonDetailAsync(string id, bool track = true);
}
