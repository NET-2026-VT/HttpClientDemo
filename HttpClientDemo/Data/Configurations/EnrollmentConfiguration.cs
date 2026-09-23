using HttpClientDemo.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace HttpClientDemo.Data.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => new { e.StudentId, e.CourseId });

        builder.Property(e => e.Grade)
              .IsRequired();
        //..

        builder.HasOne(e => e.Student)
              .WithMany(s => s.Enrollments)
              .HasForeignKey(e => e.StudentId);

        builder.HasOne(e => e.Course)
               .WithMany(c => c.Enrollments)
               .HasForeignKey(e => e.CourseId);

        builder.ToTable("Enrollment");

    }
}
