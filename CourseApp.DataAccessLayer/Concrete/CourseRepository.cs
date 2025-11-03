using CourseApp.DataAccessLayer.Abstract;
using CourseApp.EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.DataAccessLayer.Concrete;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    private readonly AppDbContext _context;
    public CourseRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public IQueryable<Course> GetAllCourseDetail(bool track = true)
    {
        var query = _context.Courses.AsQueryable();
        if(!track)
        {
            query = query.AsNoTracking();
        }
        return query.Include(c => c.Instructor);
    }

    public void InvalidMethod()
    {
        var x = MissingCourseHelper.Process();
    }
}
