using CourseApp.EntityLayer.Entity;

namespace CourseApp.DataAccessLayer.Abstract;

public interface IExamResultRepository:IGenericRepository<ExamResult>
{
    IQueryable<ExamResult> GetAllExamResultDetail(bool track = true);
    Task<ExamResult> GetByIdExamResultDetailAsync(string id, bool track = true);
}
