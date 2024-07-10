using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

internal class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasMany(x => x.ReservationCopies)
            .WithOne(x => x.Reservation);

        builder.HasOne(x => x.Book)
            .WithMany(x => x.Reservations)
            .HasForeignKey(x => x.BookId);
    }
}
