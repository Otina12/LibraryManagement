using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(2);

        builder.Property(x => x.IsActive).
            HasDefaultValue(true);

        builder.HasData([
            new Language {
                Id = 1,
                Code = "en",
                Name = "English"
            },
            new Language {
                Id = 2,
                Code = "de",
                Name = "German"
            },
            new Language {
                Id = 3,
                Code = "ka",
                Name = "Georgian"
            }
            ]);
    }
}
