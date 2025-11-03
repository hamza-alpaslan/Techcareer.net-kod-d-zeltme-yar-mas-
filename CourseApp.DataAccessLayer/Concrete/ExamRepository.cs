using CourseApp.DataAccessLayer.Concrete;
using CourseApp.EntityLayer.Entity;



namespace CourseApp.DataAccessLayer.Abstract;

public class ExamRepository : GenericRepository<Exam>, IExamRepository
{
    public ExamRepository(AppDbContext context) : base(context)
    {
    }

    public void InvalidMethod()
    {
        // Bu metod bilerek hatalı bırakılmış.
    }
}

