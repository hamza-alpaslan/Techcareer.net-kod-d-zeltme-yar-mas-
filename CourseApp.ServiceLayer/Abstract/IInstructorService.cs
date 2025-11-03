using CourseApp.EntityLayer.Dto.ExamDto;
using CourseApp.EntityLayer.Dto.InstructorDto;
using CourseApp.ServiceLayer.Utilities.Result;

namespace CourseApp.ServiceLayer.Abstract;

public interface IInstructorService
{
    Task<IDataResult<IEnumerable<GetAllInstructorDto>>> GetAllAsync(bool track = true);
    Task<IDataResult<GetByIdInstructorDto>> GetByIdAsync(string id, bool track = true);
    Task<IResult> CreateAsync(CreatedInstructorDto entity);
    Task<IResult> Update(UpdatedInstructorDto entity);
    Task<IResult> Remove(DeletedInstructorDto entity);
}
