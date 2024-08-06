using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class BookCopyLogConfiguration : IEntityTypeConfiguration<BookCopyLog>
{
    public void Configure(EntityTypeBuilder<BookCopyLog> builder)
    {
        builder.Property(x => x.BookCopyId).IsRequired();

        builder
            .HasOne(x => x.BookCopy)
            .WithMany(x => x.BookCopyLogs)
            .HasForeignKey(x => x.BookCopyId);
    }
}
