using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.Property(a => a.Name).IsRequired();
        builder.Property(a => a.Surname).IsRequired();

        builder.HasMany(a => a.Books)
            .WithMany(b => b.Authors)
            .UsingEntity(nameof(BookAuthor));
    }
}
