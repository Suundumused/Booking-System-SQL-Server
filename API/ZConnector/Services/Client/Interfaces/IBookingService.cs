using ZConnector.Models.Client;
using ZConnector.Models.Entities;


namespace ZConnector.Services.Client.Interfaces
{
    public interface IBookingService
    {
        Task<Booking?> GetBookingById(int id);

        Task<IEnumerable<Booking>> ListBookingsByHotel(int hotelId);
        Task<IEnumerable<Booking>> ListBooKingsByUserByHotel(int userId, int hotelId);
        Task<IEnumerable<Booking>> ListBookingsByDateInOutHotelAsync(DateTime checkIn, DateTime checkOut, int? hotelId);

        Task Book(BookingModel booking);
        Task UnBook(int id);
        Task UpdateBookingInfo(BookingModel bookingModel);
    }
}
