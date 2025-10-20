using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZConnector.Models.Entities;

namespace ZConnector.Data.EntityConfig
{
    public class BookingMap : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(e => e.ID);

            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserID);

            builder.HasOne(h => h.Hotel)
                .WithMany()
                .HasForeignKey(h => h.HotelID);
        }
    }
}