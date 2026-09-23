namespace HttpClientDemo.Models.Entities;

public class Assignment
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;

    public int StudentId { get; set; }

    //Conv 1
    //Conv 2 inget nav prop här
    //Conv 3
    public Student Student { get; set; } = null!;

    //Conv 4 conv 3 plus add FK



}
