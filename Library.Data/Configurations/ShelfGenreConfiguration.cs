using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class ShelfGenreConfiguration : IEntityTypeConfiguration<ShelfGenre>
{
    public void Configure(EntityTypeBuilder<ShelfGenre> builder)
    {
        builder.HasKey(sg => new { sg.GenreId, sg.ShelfId, sg.RoomId });
    }
}
