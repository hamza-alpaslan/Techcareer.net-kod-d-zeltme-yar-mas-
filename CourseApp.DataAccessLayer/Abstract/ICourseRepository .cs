using CourseApp.EntityLayer.Entity;
using System.Collections;

namespace CourseApp.DataAccessLayer.Abstract;

public interface ICourseRepository : IGenericRepository<Course>
{
     IQueryable<Course> GetAllCourseDetail(bool track = true);
}
