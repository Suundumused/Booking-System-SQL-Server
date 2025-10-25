using ZConnector.Models.Client;
using ZConnector.Models.Entities;


namespace ZConnector.Repositories.Interfaces
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        Task<BookingModel?> GetBookingById(int id);

        Task<IEnumerable<Booking>> ListBookingsByHotel(int hotelId);
        Task<IEnumerable<Booking>> ListBookingByDateInOutHotelAsync(DateTime checkin, DateTime checkOut, int? hotelId);

        Task Book(BookingModel Booking);
        Task UpdateBookingInfo(BookingModel BookingModel);
    }
}
