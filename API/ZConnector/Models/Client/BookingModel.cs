namespace ZConnector.Models.Client
{
    public class BookingModel
    {
        public int ID { get; set; }

        public required DateTime? CheckIn { get; set; }
        public required DateTime? CheckOut { get; set; }

        public required int? UserID { get; set; }
        public required int? HotelID { get; set; }
    }
}
