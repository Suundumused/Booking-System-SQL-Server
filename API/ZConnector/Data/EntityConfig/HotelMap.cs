using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ZConnector.Models.Entities;


namespace ZConnector.Data.EntityConfig
{
    public class HotelMap : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasKey(e => e.ID);
        }
    }
}
