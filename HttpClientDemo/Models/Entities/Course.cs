namespace HttpClientDemo.Models.Entities;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;

    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
