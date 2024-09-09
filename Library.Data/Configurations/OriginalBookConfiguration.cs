using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class OriginalBookConfiguration : IEntityTypeConfiguration<OriginalBook>
{
    public void Configure(EntityTypeBuilder<OriginalBook> builder)
    {
        builder.HasMany(x => x.Books)
            .WithOne(x => x.OriginalBook);
    }
}
