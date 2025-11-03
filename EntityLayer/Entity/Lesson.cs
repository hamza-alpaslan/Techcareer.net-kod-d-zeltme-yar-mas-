namespace CourseApp.EntityLayer.Entity;

public class Lesson : BaseEntity
{
    public string? Title { get; set; }
    public DateTime Date { get; set; }
    public byte Duration { get; set; }
    public string? Content { get; set; }
    public string? CourseID { get; set; }
    public string? Time { get; set; }

    //navigation property
    public Course? Course { get; set; }
}
