namespace CourseApp.EntityLayer.Entity;

public class Registration : BaseEntity
{
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    public decimal Price { get; set; }
    public string? StudentID { get; set; }
    public string? CourseID { get; set; }
    public Course? Course { get; set; }
    public Student? Student { get; set; }
}
