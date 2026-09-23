using System.ComponentModel.DataAnnotations;

namespace HttpClientDemo.Models.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;

    //FK
    public int AuthorId { get; set; }

    //Navigation prop
    public Author Author { get; set; } = null!;
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    //...
    //...
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
