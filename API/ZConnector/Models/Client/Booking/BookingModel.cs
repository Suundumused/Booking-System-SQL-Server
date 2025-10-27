using System.ComponentModel.DataAnnotations;


namespace ZConnector.Models.Client.Booking
{
    public class BookingModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Check-in date is required.")]
        public DateTime? CheckIn { get; set; }

        [Required(ErrorMessage = "Check-out date is required.")]
        public DateTime? CheckOut { get; set; }

        public int? UserID { get; set; }

        [Required(ErrorMessage = "No hotel selected.")]
        public int? HotelID { get; set; }
    }
}
