using AutoMapper;

using ZConnector.Data;
using ZConnector.Models.Client;
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

        public async Task Book(BookingModel Booking)
        {
            throw new NotImplementedException();
        }

        public async Task<BookingModel?> GetBookingById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Booking>> ListBookingByDateInOutHotelAsync(DateTime checkin, DateTime checkOut, int? hotelId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Booking>> ListBookingsByHotel(int hotelId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateBookingInfo(BookingModel BookingModel)
        {
            throw new NotImplementedException();
        }
    }
}
