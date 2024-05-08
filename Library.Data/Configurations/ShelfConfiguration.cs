using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class ShelfConfiguration : IEntityTypeConfiguration<Shelf>
{
    public void Configure(EntityTypeBuilder<Shelf> builder)
    {
        builder.HasKey(x => new { x.RoomId, x.Id });

        builder.HasMany(s => s.BookCopies)
            .WithOne(b => b.Shelf)
            .HasForeignKey(b => new { b.RoomId, b.ShelfId });

        builder.HasOne(s => s.Room)
            .WithMany(r => r.Shelves)
            .HasForeignKey(s => s.RoomId);
    }
}
