
using Bogus;
using HttpClientDemo.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HttpClientDemo.Data;

public class SeedData
{
    private static Faker faker = new Faker("sv");
    internal static async Task InitAsync(UniversityContext context)
    {
        if (await context.Students.AnyAsync()) return;

        var courses = GenerateCourses(30);
        await context.AddRangeAsync(courses);

        var students = GenerateStudents(100);
        await context.AddRangeAsync(students);

        var enrollments = GenerateEnrollments(students, courses);
        await context.AddRangeAsync(enrollments);

        await context.SaveChangesAsync();
    }

    private static IEnumerable<Enrollment> GenerateEnrollments(IEnumerable<Student> students, IEnumerable<Course> courses)
    {
        return students.SelectMany(student => courses
                                .Where(_ => faker.Random.Int(0, 5) == 0)
                                .Select(course => new Enrollment
                                {
                                    Student = student,
                                    Course = course,
                                    Grade = faker.Random.Int(1, 5)

                                }))
                                .ToList();

        //var rnd = new Random();
        //var enrollments = new List<Enrollment>();

        //foreach (var student in students)
        //{
        //    foreach (var course in courses)
        //    {
        //        if(rnd.Next(0, 5) == 0)
        //        {
        //            var enrollment = new Enrollment
        //            {
        //                Student = student,
        //                Grade = rnd.Next(1, 6),
        //                Course = course
        //            };

        //            enrollments.Add(enrollment);
        //        }
        //    }
        //}

        //return enrollments;
    }

    private static IEnumerable<Student> GenerateStudents(int numberOfStudents)
    {
        var students = new List<Student>(numberOfStudents);

        for (int i = 0; i < numberOfStudents; i++)
        {
            //int numCourses = faker.Random.Int(0, courses.Count);
            //var assignedCourses = faker.PickRandom(courses, numCourses).ToList();

            var fName = faker.Name.FirstName();
            var lName = faker.Name.LastName();
            var avatar = faker.Internet.Avatar();

            var student = new Student()
            {
                FirstName = fName,
                LastName = lName,
                Avatar = avatar,
                Address = new Address
                {
                    City = faker.Address.City(),
                    Street = faker.Address.StreetName(),
                    ZipCode = faker.Address.ZipCode()
                },
                // Courses = assignedCourses
            };

            students.Add(student);
        }

        return students;
    }

    private static IEnumerable<Course> GenerateCourses(int numberOfCourses) =>
        Enumerable.Range(1, numberOfCourses)
                  .Select(_ => new Course()
                  {
                      Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Company.Bs())
                  })
                  .ToList();
    //{
    //    var courses = new List<Course>();

    //    for (int i = 0; i < numberOfCourses; i++)
    //    {
    //        var title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Company.Bs());
    //        var course = new Course { Title = title };
    //        courses.Add(course);
    //    }

    //    return courses;
    //}
}
