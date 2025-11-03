namespace CourseApp.EntityLayer.Entity;

public class ExamResult:BaseEntity
{
    public byte Grade { get; set; }
    public string? ExamID { get; set; }
    public string? StudentID { get; set; }
    public Student? Student { get; set; }
    public Exam? Exam { get; set; }
}
