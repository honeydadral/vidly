using System.ComponentModel.DataAnnotations; 
namespace Vidly.Models
{
   public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime ReleaseDate { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        [Range(0, 1000)]
        public byte NumberInStock { get; set; }

        [Required]
        public byte? GenreId { get; set; }

        public Genre? Genre { get; set; }  // Navigation property (EF)

        // Optional – only if rating is important business data
        public string? Rating { get; set; }           // e.g. "PG-13", "R"
    }
}