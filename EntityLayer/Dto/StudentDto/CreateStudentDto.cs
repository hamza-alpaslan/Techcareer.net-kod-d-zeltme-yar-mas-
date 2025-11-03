namespace CourseApp.EntityLayer.Dto.StudentDto;

public class CreateStudentDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Fullname => $"{Name} {Surname}";
    public DateTime BirthDate { get; set; }
    public string? TC { get; set; }
}
