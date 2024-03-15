using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60)]
        public string Name { get; set; }
    }
}
