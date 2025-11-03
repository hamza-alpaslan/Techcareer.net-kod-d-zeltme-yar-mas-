namespace CourseApp.EntityLayer.Dto.CourseDto;

public class DeleteCourseDto
{
    public string Id { get; set; } = null!;
    public bool IsActive { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CourseName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? InstructorID { get; set; }
}
