using System.ComponentModel.DataAnnotations;

namespace BlazorMovies.Shared.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This is field is required")]
        public string Name { get; set; }
    }
}
