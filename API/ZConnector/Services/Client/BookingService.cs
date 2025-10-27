using AutoMapper;
using ZConnector.Models.Client.Booking;
using ZConnector.Models.Entities;
using ZConnector.Repositories.Interfaces;
using ZConnector.Services.Client.Interfaces;


namespace ZConnector.Services.Client
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;


        public BookingService(IBookingRepository bookingRepository, IMapper mapper) 
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task Book(BookingModel booking)
        {
            await _bookingRepository.Book(booking);
        }

        public async Task UnBook(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Booking?> GetBookingById(int id)
        {
            return await _bookingRepository.GetBookingById(id);
        }

        public async Task<int?> GetBookingOwnerById(int id) 
        {
            return await _bookingRepository.GetBookingOwnerById(id);
        }

        public async Task<IEnumerable<Booking>> ListBookingsByDateInOutHotelAsync(DateTime checkIn, DateTime checkOut, int? hotelId)
        {
            return await _bookingRepository.ListBookingsByDateInOutHotelAsync(checkIn, checkOut, hotelId);
        }

        public async Task<IEnumerable<Booking>> ListBookingsByHotel(int hotelId)
        {
            return await _bookingRepository.ListBookingsByHotel(hotelId);
        }

        public async Task<IEnumerable<Booking>> ListBooKingsByUserByHotel(int userId, int hotelId)
        {
            return await _bookingRepository.ListBooKingsByUserByHotel(userId, hotelId);
        }

        public async Task UpdateBookingInfo(BookingUpdateModel bookingModel)
        {
            await _bookingRepository.UpdateBookingInfo(bookingModel);
        }
    }
}
