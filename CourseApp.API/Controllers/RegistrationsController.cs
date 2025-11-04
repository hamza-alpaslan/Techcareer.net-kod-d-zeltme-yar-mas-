using CourseApp.EntityLayer.Dto.RegistrationDto;
using CourseApp.ServiceLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationsController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public RegistrationsController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _registrationService.GetAllAsync();
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _registrationService.GetByIdAsync(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetAllDetail()
    {
        var result = await _registrationService.GetAllRegistrationDetailAsync();
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetByIdDetail(string id)
    {
        var result = await _registrationService.GetByIdRegistrationDetailAsync(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRegistrationDto createRegistrationDto)
    {
        // ORTA: Null check eksik - createRegistrationDto null olabilir
        if (createRegistrationDto == null)
        {
            return BadRequest("Kayıt bilgileri gönderilmedi.");
        }
        // ORTA: Tip dönüşüm hatası - decimal'i int'e direkt cast
        if (createRegistrationDto.Price <= 0)
        {
            return BadRequest("Geçersiz fiyat değeri girildi.");
        }
        var invalidPrice = Convert.ToInt32(Math.Round(createRegistrationDto.Price)); // ORTA: InvalidCastException
        
        var result = await _registrationService.CreateAsync(createRegistrationDto);
        // KOLAY: Değişken adı typo - result yerine rsult
        if (result.Success) // TYPO: result yerine rsult
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatedRegistrationDto updatedRegistrationDto)
    {
        var result = await _registrationService.Update(updatedRegistrationDto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteRegistrationDto deleteRegistrationDto)
    {
        var result = await _registrationService.Remove(deleteRegistrationDto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
