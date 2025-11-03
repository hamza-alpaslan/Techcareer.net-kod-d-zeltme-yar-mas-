namespace CourseApp.EntityLayer.Dto.RegistrationDto;

public class GetByIdRegistrationDto
{
    public string Id { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    public decimal Price { get; set; }
    public string? StudentID { get; set; }
    public string? CourseID { get; set; }
}
