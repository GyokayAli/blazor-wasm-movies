using BlazorMovies.Shared.Entities;
using System.Collections.Generic;

namespace BlazorMovies.Shared.DTO
{
    public class MovieDetailsDTO
    {
        public Movie Movie { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Person> Actors { get; set; }
        public List<Theater> Theaters { get; set; }
        public double AverageVote { get; set; }
        public int UserVote { get; set; }
    }
}
