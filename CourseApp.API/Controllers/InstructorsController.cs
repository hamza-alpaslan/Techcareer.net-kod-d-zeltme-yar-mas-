using CourseApp.EntityLayer.Dto.InstructorDto;
using CourseApp.ServiceLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorsController : ControllerBase
{
    private readonly IInstructorService _instructorService;

    public InstructorsController(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _instructorService.GetAllAsync();
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _instructorService.GetByIdAsync(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatedInstructorDto createdInstructorDto)
    {
        // ORTA: Null check eksik - createdInstructorDto null olabilir
        if (createdInstructorDto == null)
            return BadRequest("Eğitmen bilgisi boş olamaz.");

        if (string.IsNullOrWhiteSpace(createdInstructorDto.Name))
            return BadRequest("Eğitmen adı boş olamaz.");
        var instructorName = createdInstructorDto.Name; // Null reference riski

        // ORTA: Index out of range - instructorName boş/null ise
        char firstChar = instructorName.FirstOrDefault(); // IndexOutOfRangeException riski

        // ORTA: Tip dönüşüm hatası - string'i int'e direkt cast
        //var invalidAge = Convert.ToInt32(instructorName); // ORTA: InvalidCastException

        var result = await _instructorService.CreateAsync(createdInstructorDto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatedInstructorDto updatedInstructorDto)
    {
        var result = await _instructorService.Update(updatedInstructorDto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeletedInstructorDto deletedInstructorDto)
    {
        var result = await _instructorService.Remove(deletedInstructorDto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
