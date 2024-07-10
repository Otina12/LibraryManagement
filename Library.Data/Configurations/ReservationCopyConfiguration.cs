using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class ReservationCopyConfiguration : IEntityTypeConfiguration<ReservationCopy>
{
    public void Configure(EntityTypeBuilder<ReservationCopy> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Reservation)
            .WithMany(x => x.ReservationCopies)
            .HasForeignKey(x => x.ReservationId);
    }
}
