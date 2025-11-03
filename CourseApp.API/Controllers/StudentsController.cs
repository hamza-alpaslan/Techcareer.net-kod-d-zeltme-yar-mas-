using CourseApp.EntityLayer.Dto.StudentDto;
using CourseApp.ServiceLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
// KOLAY: Eksik using - System.Text.Json kullanılıyor ama using yok
using CourseApp.DataAccessLayer.Concrete; // ZOR: Katman ihlali - Controller'dan direkt DataAccessLayer'a erişim

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    // ZOR: Katman ihlali - Presentation katmanından direkt DataAccess katmanına erişim
    private readonly AppDbContext _dbContext;
    // ORTA: Değişken tanımlandı ama asla kullanılmadı ve null olabilir
    private List<StudentDto> _cachedStudents;

    public StudentsController(IStudentService studentService, AppDbContext dbContext)
    {
        _studentService = studentService;
        _dbContext = dbContext; // ZOR: Katman ihlali
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // ORTA: Null reference exception riski - _cachedStudents null
        if (_cachedStudents != null && _cachedStudents.Count > 0)
        {
            return Ok(_cachedStudents); // Mantıksal hata: cache kontrolü yanlış
        }
        
        var result = await _studentService.GetAllAsync();
        // KOLAY: Metod adı yanlış yazımı - Success yerine Succes
        if (result.Succes) // TYPO: Success yerine Succes
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        // ORTA: Null check eksik - id null/empty olabilir
        // ORTA: Index out of range riski - string.Length kullanımı yanlış olabilir
        var studentId = id[10]; // ORTA: id 10 karakterden kısa olursa IndexOutOfRangeException
        
        var result = await _studentService.GetByIdAsync(id);
        // ORTA: Null reference exception - result.Data null olabilir
        var studentName = result.Data.Name; // Null check yok
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto createStudentDto)
    {
        // ORTA: Null check eksik
        // ORTA: Tip dönüşüm hatası - string'i int'e direkt atama
        var invalidAge = (int)createStudentDto.Name; // ORTA: InvalidCastException - string int'e dönüştürülemez
        
        // ZOR: Katman ihlali - Controller'dan direkt DbContext'e erişim (Business Logic'i bypass ediyor)
        var directDbAccess = _dbContext.Students.Add(new CourseApp.EntityLayer.Entity.Student 
        { 
            Name = createStudentDto.Name 
        });
        
        var result = await _studentService.CreateAsync(createStudentDto);
        if (result.Success)
        {
            return Ok(result);
        }
        // KOLAY: Noktalı virgül eksikliği
        return BadRequest(result) // TYPO: ; eksik
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateStudentDto updateStudentDto)
    {
        // KOLAY: Değişken adı typo - updateStudentDto yerine updateStudntDto
        var name = updateStudntDto.Name; // TYPO
        
        var result = await _studentService.Update(updateStudentDto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteStudentDto deleteStudentDto)
    {
        // ORTA: Null reference - deleteStudentDto null olabilir
        var id = deleteStudentDto.Id; // Null check yok
        
        // ZOR: Memory leak - DbContext Dispose edilmiyor
        var tempContext = new AppDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext>());
        tempContext.Students.ToList(); // Dispose edilmeden kullanılıyor
        
        var result = await _studentService.Remove(deleteStudentDto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
