namespace CourseApp.EntityLayer.Dto.StudentDto;

public class GetByIdStudentDto
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Fullname => $"{Name} {Surname}";
    public DateTime BirthDate { get; set; }
    public string? TC { get; set; }
}
