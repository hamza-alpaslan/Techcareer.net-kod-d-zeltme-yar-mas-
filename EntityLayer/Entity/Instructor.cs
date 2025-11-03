namespace CourseApp.EntityLayer.Entity;

public class Instructor : BaseEntity
{
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

    //navigation property
    public IQueryable<Course>? Courses { get; set; }
}
