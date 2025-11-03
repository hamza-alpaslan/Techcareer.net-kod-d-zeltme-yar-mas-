using CourseApp.DataAccessLayer.Abstract;
using CourseApp.EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.DataAccessLayer.Concrete;

public class ExamResultRepository : GenericRepository<ExamResult>, IExamResultRepository
{
    private readonly AppDbContext _context;
    private  DbSet<ExamResult> _dbSet => _context.Set<ExamResult>();
    public ExamResultRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public IQueryable<ExamResult> GetAllExamResultDetail(bool track = true)
    {
        var query = _dbSet.AsQueryable();
        if(!track)
        {
            query = query.AsNoTracking();
        }
        return query.Include(er => er.Student)
                    .Include(er => er.Exam);
    }

    public async Task<ExamResult> GetByIdExamResultDetailAsync(string id, bool track = true)
    {
        var query = _dbSet.AsQueryable();
        if (!track)
        {
            query = query.AsNoTracking();
        }

        return (await query.Include(er => er.Student)
                            .Include(er => er.Exam)
                            .FirstOrDefaultAsync(er => er.ID == id))!;
    }

    private void UseMissingType()
    {
        var helper = ExamResultHelper.Process();
    }
}
