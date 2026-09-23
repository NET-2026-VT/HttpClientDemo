using HttpClientDemo.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HttpClientDemo.Data.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        
        builder.HasIndex(a => a.StudentId)
               .IsUnique(); //This ensures one-to-one

        builder.ToTable("Address");

    }
}
