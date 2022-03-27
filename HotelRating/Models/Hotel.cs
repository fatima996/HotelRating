using System.ComponentModel.DataAnnotations;

namespace HotelRating.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Details { get; set; }
        public string? Image { get; set; } = "https://images.unsplash.com/photo-1564501049412-61c2a3083791?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1932&q=80";
        public bool IsHalal { get; set; }

    }
}
