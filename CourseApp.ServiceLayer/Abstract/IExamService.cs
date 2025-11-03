using CourseApp.EntityLayer.Dto.CourseDto;
using CourseApp.EntityLayer.Dto.ExamDto;
using CourseApp.ServiceLayer.Utilities.Result;

namespace CourseApp.ServiceLayer.Abstract;

public interface IExamService
{
    Task<IDataResult<IEnumerable<GetAllExamDto>>> GetAllAsync(bool track = true);
    Task<IDataResult<GetByIdExamDto>> GetByIdAsync(string id, bool track = true);
    Task<IResult> CreateAsync(CreateExamDto entity);
    Task<IResult> Update(UpdateExamDto entity);
    Task<IResult> Remove(DeleteExamDto entity);
}
