using BlazorMovies.Shared.Entities;
using System.Collections.Generic;

namespace BlazorMovies.Shared.DTO
{
    public class PersonDetailsDTO
    {
        public Person Person { get; set; }
        public List<Movie> Movies { get; set; }
        public string FeaturedMovieTrailer { get; set; }
    }
}
