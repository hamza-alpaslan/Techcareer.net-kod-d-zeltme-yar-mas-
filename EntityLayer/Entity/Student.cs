namespace CourseApp.EntityLayer.Entity;

public class Student : BaseEntity
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Fullname => $"{Name} {Surname}";
    public DateTime BirthDate { get; set; }
    public string? TC { get; set; }

    public override string ToString()
    {
        return $"{TC}-{Fullname}";
    }
}
