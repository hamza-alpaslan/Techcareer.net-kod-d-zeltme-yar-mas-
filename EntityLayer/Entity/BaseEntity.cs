namespace CourseApp.EntityLayer.Entity;

public abstract class BaseEntity
{
    public string ID { get; set; } = Guid.NewGuid().ToString();
    public bool IsActive { get; set; } = false; 
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
