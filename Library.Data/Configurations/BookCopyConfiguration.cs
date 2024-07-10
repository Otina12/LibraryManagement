using Library.Model.Enums;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class BookCopyConfiguration : IEntityTypeConfiguration<BookCopy>
{
    public void Configure(EntityTypeBuilder<BookCopy> builder)
    {
        builder.Property(b => b.BookId).IsRequired();
        builder.Property(b => b.Status)
            .HasDefaultValue(Status.Normal);

        builder.HasMany(b => b.ReservationCopies)
            .WithOne(r => r.BookCopy)
            .HasForeignKey(b => b.BookCopyId);

        builder.HasOne(b => b.Shelf)
            .WithMany(s => s.BookCopies)
            .HasForeignKey(b => new { b.ShelfId, b.BookId });
    }
}
