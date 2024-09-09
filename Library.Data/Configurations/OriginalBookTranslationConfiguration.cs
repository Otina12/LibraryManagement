using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class OriginalBookTranslationConfiguration : IEntityTypeConfiguration<OriginalBookTranslation>
{
    public void Configure(EntityTypeBuilder<OriginalBookTranslation> builder)
    {
        builder.HasKey(x => new { x.OriginalBookId, x.LanguageId});

        builder.Property(x => x.Title).IsRequired();
    }
}
