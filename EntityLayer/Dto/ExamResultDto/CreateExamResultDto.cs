namespace CourseApp.EntityLayer.Dto.ExamResultDto;

public class CreateExamResultDto
{
    public byte Grade { get; set; }
    public string? ExamID { get; set; }
    public string? StudentID { get; set; }
}
