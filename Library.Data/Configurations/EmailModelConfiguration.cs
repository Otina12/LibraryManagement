using Library.Model.Models.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class EmailModelConfiguration : IEntityTypeConfiguration<EmailModel>
{
    public void Configure(EntityTypeBuilder<EmailModel> builder)
    {
        builder.Property(x => x.From).IsRequired();

        builder.Property(x => x.Body)
            .IsRequired()
            .HasMaxLength(500);
    }
}
