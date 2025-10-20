using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ZConnector.Models.Entities
{
    public class Booking
    {
        [Key]
        public int ID { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        public int UserID { get; set; }
        public virtual User User { get; set; } = null!;

        public int HotelID { get; set; }
        public virtual Hotel Hotel { get; set; } = null!;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? Price { get; set; }
    }
}
