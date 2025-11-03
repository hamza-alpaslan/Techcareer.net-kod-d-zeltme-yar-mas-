namespace CourseApp.EntityLayer.Dto.InstructorDto;

public class GetByIdInstructorDto
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    //public string FullName => $"{Name} {Surname}";
    public string FullName
    {
        get
        {
            return $"{Name} {Surname}";
        }
    }
    public string? Email { get; set; }
    public string? Professions { get; set; }
    public string? PhoneNumber { get; set; }
}
