using HttpClientDemo.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace HttpClientDemo.Data.Configurations;

public class StudentConfigurations : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Avatar)
          .HasColumnName("Avatar") //For demo
          .HasMaxLength(255);

        builder.Property<DateTime>("Edited");

        //..
        //..

         builder.HasOne(s => s.Address)
                .WithOne(a => a.Student)
                .HasForeignKey<Address>(a => a.StudentId);


        builder.HasMany(s => s.Courses)
               .WithMany(c => c.Students)
               .UsingEntity<Enrollment>(
               e => e.HasOne(e => e.Course).WithMany(c => c.Enrollments),
               e => e.HasOne(e => e.Student).WithMany(s => s.Enrollments));

        builder.ToTable("Student");
    }
}
