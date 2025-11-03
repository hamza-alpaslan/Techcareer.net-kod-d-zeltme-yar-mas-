using System.Linq.Expressions;
namespace CourseApp.DataAccessLayer.Abstract;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetAll(bool track = true);
    Task<T> GetByIdAsync(string id,bool track = true);
    IQueryable<T> Where(Expression<Func<T, bool>> predicate,bool track);
    Task CreateAsync(T entity);
    void Update(T entity);
    void Remove(T entity);

}
