using System.ComponentModel.DataAnnotations;


namespace ZConnector.Models.Client.Booking
{
    public class BookingUpdateModel
    {
        [Required(ErrorMessage = "No booking selected.")]
        public int? ID { get; set; }

        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }

        public int? UserID { get; set; }
        public int? HotelID { get; set; }
    }
}
