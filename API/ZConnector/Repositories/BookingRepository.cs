using AutoMapper;

using Microsoft.EntityFrameworkCore;

using ZConnector.Data;
using ZConnector.Models.Client.Booking;
using ZConnector.Models.Entities;
using ZConnector.Repositories.Interfaces;


namespace ZConnector.Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        private readonly IMapper _mapper;


        public BookingRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<Booking?> GetBookingById(int id)
        {
            return await _context.Bookings
                .Include(u => u.User)
                .Include(h => h.Hotel)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetBookingOwnerById(int id) 
        {
            return await _context.Bookings.Where(b => b.ID == id).Select(u => u.UserID).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Booking>> ListBookingsByDateInOutHotelAsync(DateTime checkIn, DateTime checkOut, int? hotelId)
        {
            return await _context.Bookings
                .Where(b => 
                    b.CheckIn == checkIn 
                    && 
                    b.CheckOut == checkOut 
                    && 
                    b.HotelID == hotelId
                )
                .Include(u => u.User)
                .Include(h => h.Hotel)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> ListBookingsByHotel(int hotelId)
        {
            return await _context.Bookings
                .Where(h => h.HotelID == hotelId)
                .Include(u => u.User)
                .Include(h => h.Hotel)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> ListBooKingsByUserByHotel(int userId, int hotelId) 
        {
            return await _context.Bookings
                .Where(b => b.UserID == userId && b.HotelID == hotelId)
                /*.Select(b => new Booking
                {
                    ID = b.ID,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    UserID = b.UserID,
                    HotelID = b.HotelID,
                    Price = b.Price,
                    UserModel = new UserModel
                    {
                        ID = b.User.ID,
                        Username = b.User.Username,
                        Email = b.User.Email,
                        Name = b.User.Name,
                        Phone1 = b.User.Phone1,
                        Phone2 = b.User.Phone2,
                        LastLoginDate = b.User.LastLoginDate
                    },
                    Hotel = new Hotel
                    {
                        ID = b.Hotel.ID,
                        Name = b.Hotel.Name,
                        Location = b.Hotel.Location,
                        Rooms = b.Hotel.Rooms,
                        Price = b.Hotel.Price
                    }
                })*/
                .ToListAsync();
        }

        public async Task Book(BookingModel booking)
        {
            await AddAsync(_mapper.Map<Booking>(booking));
            await SaveChangesAsync();
        }

        public async Task UpdateBookingInfo(BookingUpdateModel bookingModel)
        {
            var booking = new Booking { ID = (int)bookingModel.ID! };

            _context.Bookings.Attach(booking);

            if (bookingModel.CheckIn is not null) 
            {
                booking.CheckIn = bookingModel.CheckIn.Value;
                _context.Entry(booking).Property(i => i.CheckIn).IsModified = true;
            }

            if (bookingModel.CheckOut is not null) 
            {
                booking.CheckOut = bookingModel.CheckOut.Value;
                _context.Entry(booking).Property(o => o.CheckOut).IsModified = true;
            }

            if (bookingModel.UserID is not null) 
            {
                booking.UserID = bookingModel.UserID.Value;
                _context.Entry(booking).Property(u => u.UserID).IsModified = true;
            }

            if (bookingModel.HotelID is not null) 
            {
                booking.HotelID = bookingModel.HotelID.Value;
                _context.Entry(booking).Property(h => h.HotelID).IsModified = true;
            }

            await SaveChangesAsync();
        }
    }
}
