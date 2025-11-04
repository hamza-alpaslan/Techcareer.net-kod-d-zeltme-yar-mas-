using CourseApp.EntityLayer.Dto.StudentDto;
using CourseApp.ServiceLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using CourseApp.DataAccessLayer.Concrete; // ZOR: Katman ihlali - Controller'dan direkt DataAccessLayer'a erişim
using CourseApp.EntityLayer.Dto;
namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    // ZOR: Katman ihlali - Presentation katmanından direkt DataAccess katmanına erişim
    //private readonly AppDbContext _dbContext;
    // ORTA: Değişken tanımlandı ama asla kullanılmadı ve null olabilir
    //private List<GetAllStudentDto> _cachedStudents;

    public StudentsController(IStudentService studentService, AppDbContext dbContext)
    {
        _studentService = studentService;
        //_dbContext = dbContext; // ZOR: Katman ihlali
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {

        var result = await _studentService.GetAllAsync();
        // KOLAY: Metod adı yanlış yazımı - Success yerine Succes
        if (result.Success) // TYPO: Success yerine Succes
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        // ORTA: Null check eksik - id null/empty olabilir
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Geçersiz ID değeri.");
        // ORTA: Index out of range riski - string.Length kullanımı yanlış olabilir
        //var studentId = id[10]; // ORTA: id 10 karakterden kısa olursa IndexOutOfRangeException
        
        var result = await _studentService.GetByIdAsync(id);
        // ORTA: Null reference exception - result.Data null olabilir
        if (!result.Success || result.Data == null)
            return NotFound(result.Message);

        
        string? studentName = result.Data.Name; // Null check yok
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
        if (createStudentDto == null)
            return BadRequest("Öğrenci bilgisi boş olamaz.");
        // ORTA: Tip dönüşüm hatası - string'i int'e direkt atama
        //var invalidAge = createStudentDto.Name; // ORTA: InvalidCastException - string int'e dönüştürülemez

        // ZOR: Katman ihlali - Controller'dan direkt DbContext'e erişim (Business Logic'i bypass ediyor)
        //var directDbAccess = _dbContext.Students.Add(new CourseApp.EntityLayer.Entity.Student 
        //{ 
        //    Name = createStudentDto.Name 
        //});

        var result = await _studentService.CreateAsync(createStudentDto);
        if (result.Success)
        {
            return Ok(result);
        }
        // KOLAY: Noktalı virgül eksikliği
        return BadRequest(result); // TYPO: ; eksik
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateStudentDto updateStudentDto)
    {
        // KOLAY: Değişken adı typo - updateStudentDto yerine updateStudntDto
        if (updateStudentDto == null)
            return BadRequest("Güncellenecek öğrenci bilgisi boş olamaz.");
        var name = updateStudentDto.Name; // TYPO
        
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
                    if (deleteStudentDto == null || string.IsNullOrWhiteSpace(deleteStudentDto.Id))
                return BadRequest("Geçersiz öğrenci ID'si.");
        var id = deleteStudentDto.Id; // Null check yok
        
        // ZOR: Memory leak - DbContext Dispose edilmiyor
        //var tempContext = new AppDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext>());
        //tempContext.Students.ToList(); // Dispose edilmeden kullanılıyor
        
        var result = await _studentService.Remove(deleteStudentDto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
