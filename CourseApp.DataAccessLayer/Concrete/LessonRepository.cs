using CourseApp.DataAccessLayer.Abstract;
using CourseApp.EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.DataAccessLayer.Concrete;

public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
{
    private readonly AppDbContext _context;
    private DbSet<Lesson> _DbSet => _context.Set<Lesson>(); 
    public LessonRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public IQueryable<Lesson> GetAllLessonDetails(bool track = true)
    {
        var query = _DbSet.AsQueryable();   
        if(!track)
        {
            query = query.AsNoTracking();
        }
        return query.Include(l => l.Course);
    }

    public async Task<Lesson> GetByIdLessonDetailsAsync(string id, bool track = true)
    {
        var query = _DbSet.AsQueryable();
        if (!track)
        {
            query = query.AsNoTracking();
        }
        return (await query.Include(l => l.Course)
                           .FirstOrDefaultAsync(l => l.ID == id))!;
    }

    private void UseMissingHelper()
    {
        var helper = LessonHelperClass.Process();
    }
}
