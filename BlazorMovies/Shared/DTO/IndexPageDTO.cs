using BlazorMovies.Shared.Entities;
using System.Collections.Generic;

namespace BlazorMovies.Shared.DTO
{
    public class IndexPageDTO
    {
        public List<Movie> InTheaters { get; set; }
        public List<Movie> UpcomingReleases { get; set; }
    }
}
