using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace HttpClientDemo.Models.Entities;

[Index(nameof(StudentId), IsUnique = true)]
public class Address
{
    public int Id { get; set; }
    public string Street { get; set; }     = null!;
    public string ZipCode { get; set; }    = null!;
    public string City { get; set; }       = null!;
                                        
    //Foreign Key
    public int StudentId { get; set; }

    //Navigation property
    //[JsonIgnore]
    public Student Student { get; set; } = null!;
}
