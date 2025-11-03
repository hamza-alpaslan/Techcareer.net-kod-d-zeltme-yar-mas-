using CourseApp.DataAccessLayer.Concrete;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Concrete;
using CourseApp.ServiceLayer.Mapping;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// UnitOfWork Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Service Configuration
// KOLAY: Metod adı yanlış yazımı - AddScoped yerine AddScopd
builder.Services.AddScopd<IStudentService, StudentManager>(); // TYPO: Scoped yerine Scopd
builder.Services.AddScoped<ICourseService, CourseManager>();
// KOLAY: Yanlış tip - ExamManager yerine ExamManagr
builder.Services.AddScoped<IExamService, ExamManagr>(); // TYPO
builder.Services.AddScoped<IExamResultService, ExamResultManager>();
builder.Services.AddScoped<IInstructorService, InstructorManager>();
builder.Services.AddScoped<ILessonService, LessonsManager>();
builder.Services.AddScoped<IRegistrationService, RegistrationManager>();

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(StudentMapping).Assembly);

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

// KOLAY: Metod adı yanlış yazımı - MapControllers yerine MapContrllers
app.MapContrllers(); // TYPO: Controllers yerine Contrllers

// ZOR: Memory leak - app Dispose edilmiyor ama burada normal (app.Run() son satır)
app.Run();
// KOLAY: Noktalı virgül eksikliği yok - burada sorun yok ama ekstra bir satır var