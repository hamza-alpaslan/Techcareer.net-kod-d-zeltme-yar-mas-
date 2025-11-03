using CourseApp.DataAccessLayer.Abstract;
using CourseApp.EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseApp.DataAccessLayer.Concrete;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;
    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>(); 
    }



    public IQueryable<T> GetAll(bool track = true)
    {
        var query = _dbSet.AsQueryable();
        if (!track)
        {
            query = query.AsNoTracking();
        }
        return query;
    }
    public IQueryable<T> Where(Expression<Func<T, bool>> predicate,bool track)
    {
        var query = _dbSet.AsQueryable();
        if(!track)
        {
            query = query.AsNoTracking();
        }
        return query.Where(predicate);
    }

    public async Task<T> GetByIdAsync(string id, bool track = true)
    {
        var query = _dbSet.AsQueryable();
        if(!track)
        {
            query = query.AsNoTracking();
        }
        return (await query.FirstOrDefaultAsync(x => x.ID == id))!;
    }

    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void UseUndefinedType()
    {
        var x = new UndefinedRepositoryType();
        x.Process();
    }

}
