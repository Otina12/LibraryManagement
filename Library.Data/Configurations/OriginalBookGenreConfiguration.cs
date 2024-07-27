using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class BookGenreConfiguration : IEntityTypeConfiguration<OriginalBookGenre>
{
    public void Configure(EntityTypeBuilder<OriginalBookGenre> builder)
    {
        builder.HasKey(bg => new { bg.OriginalBookId, bg.GenreId });
    }
}
