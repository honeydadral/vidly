using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Genre
    {
        public byte Id {get; set;}

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}