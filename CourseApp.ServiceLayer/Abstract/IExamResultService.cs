using CourseApp.EntityLayer.Dto.ExamDto;
using CourseApp.EntityLayer.Dto.ExamResultDto;
using CourseApp.ServiceLayer.Utilities.Result;

namespace CourseApp.ServiceLayer.Abstract;

public interface IExamResultService
{
    Task<IDataResult<IEnumerable<GetAllExamResultDto>>> GetAllAsync(bool track = true);
    Task<IDataResult<GetByIdExamResultDto>> GetByIdAsync(string id, bool track = true);
    Task<IResult> CreateAsync(CreateExamResultDto entity);
    Task<IResult> Update(UpdateExamResultDto entity);
    Task<IResult> Remove(DeleteExamResultDto entity);
    Task<IDataResult<IEnumerable<GetAllExamResultDetailDto>>> GetAllExamResultDetailAsync(bool track = true);
    Task<IDataResult<GetByIdExamResultDetailDto>> GetByIdExamResultDetailAsync(string id, bool track = true);
}
