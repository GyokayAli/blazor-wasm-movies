namespace BlazorMovies.Shared.Entities
{
    public class MoviesTheaters
    {
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public Movie Movie { get; set; }
        public Theater Theater { get; set; }
    }
}
