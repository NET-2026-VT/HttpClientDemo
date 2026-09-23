namespace HttpClientDemo.Models.Dtos;

public class SelectManyDto
{
    public int Value { get; set; }
    public ICollection<CourseDto> CourseDtos { get; set; } = [];
}
