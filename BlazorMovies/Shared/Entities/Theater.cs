using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorMovies.Shared.Entities
{
    public class Theater
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public List<MoviesTheaters> MoviesTheaters { get; set; }

        public Theater()
        {
            MoviesTheaters = new List<MoviesTheaters>();
        }
    }
}
