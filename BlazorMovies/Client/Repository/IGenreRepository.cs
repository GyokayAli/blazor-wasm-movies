using BlazorMovies.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetGenres();
        Task CreateGenre(Genre genre);
    }
}
