using CourseApp.EntityLayer.Dto.LessonDto;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.DataAccessLayer.Abstract;

public interface ILessonRepository:IGenericRepository<Lesson>
{
    IQueryable<Lesson> GetAllLessonDetails(bool track = true);
    Task<Lesson> GetByIdLessonDetailsAsync(string id, bool track = true);

}
