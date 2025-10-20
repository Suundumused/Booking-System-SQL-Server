using System.ComponentModel.DataAnnotations;


namespace ZConnector.Models.Entities
{
    public class Hotel
    {
        [Key]
        public int ID { get; set; }

        public required string Name { get; set; }
        public required string Location { get; set; }

        public int Rooms { get; set; }
        public decimal Price { get; set; }
    }
}
