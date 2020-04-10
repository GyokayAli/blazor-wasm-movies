using BlazorMovies.Shared.Entities;
using System.Collections.Generic;

namespace BlazorMovies.Shared.DTO
{
    public class MovieUpdateDTO
    {
        public Movie Movie { get; set; }
        public List<Person> Actors { get; set; }
        public List<Theater> Theaters { get; set; }
        public List<Genre> SelectedGenres { get; set; }
        public List<Genre> NotSelectedGenres { get; set; }
    }
}
