using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Surname).IsRequired();
        builder.Property(c => c.Address).IsRequired();

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasAnnotation("Regex", "^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s\\./0-9]*$");

        builder.Property(c => c.Email)
            .HasAnnotation("Regex", "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");

        builder.HasMany(b => b.Reservations)
            .WithOne(r => r.Customer)
            .HasForeignKey(b => b.CustomerId);
    }
}
