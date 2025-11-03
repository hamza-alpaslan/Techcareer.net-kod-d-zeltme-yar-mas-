using CourseApp.EntityLayer.Entity;

namespace CourseApp.DataAccessLayer.Abstract;

public interface IRegistrationRepository:IGenericRepository<Registration>
{
    IQueryable<Registration> GetAllRegistrationDetail(bool track = true);
    Task<Registration> GetByIdRegistrationDetail(string id,bool track = true);    
}
