using ZConnector.Models.Client.Booking;
using ZConnector.Models.Entities;


namespace ZConnector.Repositories.Interfaces
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        Task<Booking?> GetBookingById(int id);
        Task<int?> GetBookingOwnerById(int id);

        Task<IEnumerable<Booking>> ListBookingsByHotel(int hotelId);
        Task<IEnumerable<Booking>> ListBooKingsByUserByHotel(int userId, int hotelId);
        Task<IEnumerable<Booking>> ListBookingsByDateInOutHotelAsync(DateTime checkIn, DateTime checkOut, int? hotelId);

        Task Book(BookingModel booking);
        Task UpdateBookingInfo(BookingUpdateModel bookingModel);
    }
}
