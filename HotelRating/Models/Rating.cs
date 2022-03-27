using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelRating.Models
{
    public class Rating
    {

        [Key]
        public int Id { get; set; }
        public string Username  { get; set; }
        public string Comment { get; set; }
        public int Rate { get; set; }
        public int HotelId { get; set; }
    }
}
