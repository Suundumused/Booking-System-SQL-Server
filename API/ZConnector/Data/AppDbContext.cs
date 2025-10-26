using Microsoft.EntityFrameworkCore;

using ZConnector.Data.EntityConfig;
using ZConnector.Models.Entities;


namespace ZConnector.Data 
{
    public partial class AppDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
    

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new HotelMap());
            modelBuilder.ApplyConfiguration(new BookingMap());
        }
    }
}