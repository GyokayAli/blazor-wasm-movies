using BlazorMovies.Shared.Entities;
using System.Collections.Generic;

namespace BlazorMovies.Shared.DTO
{
    public class TheaterDetailsDTO
    {
        public Theater Theater { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
