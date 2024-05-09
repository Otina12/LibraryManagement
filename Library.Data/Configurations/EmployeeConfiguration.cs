using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Surname).IsRequired();

        builder.Property(e => e.DateOfBirth)
            .HasColumnType("date");

        builder.Property(e => e.Email)
            .HasAnnotation("Regex", "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")
            .IsRequired();

        builder.Property(e => e.PhoneNumber)
            .IsRequired()
            .HasAnnotation("Regex", "^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s\\./0-9]*$");
    }
}
