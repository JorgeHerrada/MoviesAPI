using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class GenreCreationDTO
    {
        [Required]
        [StringLength(60)]
        public string Name { get; set; }
    }
}
