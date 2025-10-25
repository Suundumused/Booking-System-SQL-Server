using ZConnector.Models.Entities;

namespace ZConnector.Models.Client
{
    public class BookingModel
    {
        public required DateTime CheckIn { get; set; }
        public required DateTime CheckOut { get; set; }

        public required int UserID { get; set; }
        public required int HotelID { get; set; }
    }
}
