namespace CourseApp.EntityLayer.Dto.ExamResultDto;

public class GetAllExamResultDetailDto
{
    public string Id { get; set; } = null!;
    public byte Grade { get; set; }
    public string StudentName { get; set; } = null!;
    public string StudentSurname { get; set; } = null!;
    public string StudentFullName => $"{StudentName} {StudentSurname}";
    public string ExamName { get; set; } = null!;
    public string? ExamID { get; set; }
    public string? StudentID { get; set; }
}
