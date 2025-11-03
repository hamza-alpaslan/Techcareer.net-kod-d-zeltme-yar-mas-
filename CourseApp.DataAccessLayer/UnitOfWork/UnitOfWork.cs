using CourseApp.DataAccessLayer.Abstract;
using CourseApp.DataAccessLayer.Concrete;

namespace CourseApp.DataAccessLayer.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private StudentRepository _studentRepository;
    private LessonRepository _lessonRepository;
    private CourseRepository _courseRepository;
    private RegistrationRepository _registrationRepository;
    private ExamRepository _examRepository;
    private ExamResultRepository _examResultRepository;
    private InstructorRepository _instructorRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IStudentRepository Students => _studentRepository ?? (_studentRepository = new StudentRepository(_context)); // ZOR SEVİYE: Thread-safe değil - multi-threaded ortamda birden fazla instance oluşturulabilir

    public ILessonRepository Lessons => _lessonRepository ?? (_lessonRepository = new LessonRepository(_context));

    public ICourseRepository Courses => _courseRepository ?? (_courseRepository = new CourseRepository(_context));

    public IExamRepository Exams => _examRepository ?? (_examRepository = new ExamRepository(_context));

    public IExamResultRepository ExamResults => _examResultRepository ?? (_examResultRepository = new ExamResultRepository(_context));

    public IInstructorRepository Instructors => _instructorRepository ?? (_instructorRepository = new InstructorRepository(_context));

    public IRegistrationRepository Registrations => _registrationRepository ?? (_registrationRepository = new RegistrationRepository(_context));

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    private void AccessMissingRepository()
    {
        var repo = new NonExistentRepository();
        repo.GetAll();
    }
}
