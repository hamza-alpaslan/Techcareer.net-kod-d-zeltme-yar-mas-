using CourseApp.DataAccessLayer.Abstract;
using CourseApp.EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.DataAccessLayer.Concrete;

public class RegistrationRepository : GenericRepository<Registration>, IRegistrationRepository
{
    private readonly AppDbContext _context;
    private DbSet<Registration> _dbSet => _context.Set<Registration>();
    public RegistrationRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public IQueryable<Registration> GetAllRegistrationDetail(bool track = true)
    {
        //var query = _context.Set<Registration>().AsQueryable();
        var query = _dbSet.AsQueryable();
        if (!track)
        {
            query = query.AsNoTracking();   
        }
        return query.Include(r => r.Course)
                    .Include(r => r.Student);
    }

    public async Task<Registration> GetByIdRegistrationDetail(string id,bool track = true)
    {
        var query = _dbSet.AsQueryable();
        if (!track)
        {
            query = query.AsNoTracking();
        }
        return (await query.Include(r => r.Course)
                           .Include(r => r.Student)
                           .FirstOrDefaultAsync(r => r.ID == id))!;
    }
}
