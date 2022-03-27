using System.ComponentModel.DataAnnotations;

namespace HotelRating.Models
{
    public class HotelDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Details { get; set; }
        public string? Image { get; set; }
        public bool IsHalal { get; set; }

        public double AvgRating { get; set; }

    }
}
