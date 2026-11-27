namespace CodeFirstLab.Models;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Credit { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}