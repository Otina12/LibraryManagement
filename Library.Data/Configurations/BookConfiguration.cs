using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(b => b.Title).IsRequired();
        builder.Property(b => b.ISBN).HasMaxLength(13);

        builder.HasMany(b => b.Genres)
            .WithMany(g => g.Books)
            .UsingEntity(nameof(BookGenre));

        builder.HasOne(b => b.Publisher)
            .WithMany(p => p.BooksPublished)
            .HasForeignKey(b => b.PublisherId);

        builder.HasMany(b => b.BookCopies)
            .WithOne(bc => bc.Book)
            .HasForeignKey(b => b.BookId);
    }
}
