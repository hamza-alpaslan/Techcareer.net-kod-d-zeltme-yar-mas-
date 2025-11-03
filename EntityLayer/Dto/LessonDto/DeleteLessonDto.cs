namespace CourseApp.EntityLayer.Dto.LessonDto;

public class DeleteLessonDto
{
    public string Id { get; set; }
    public string? Title { get; set; }
    public DateTime Date { get; set; }
    public byte Duration { get; set; }
    public string? Content { get; set; }
    public string? CourseID { get; set; }
    public string? Time { get; set; }
}
