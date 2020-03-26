using BlazorMovies.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetGenres();
        Task<Genre> GetGenre(int id);
        Task CreateGenre(Genre genre);
        Task UpdateGenre(Genre genre);
    }
}
