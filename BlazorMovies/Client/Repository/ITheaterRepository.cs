using BlazorMovies.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface ITheaterRepository
    {
        Task<List<Theater>> GetTheaters();
        Task<Theater> GetTheater(int id);
        Task CreateTheater(Theater theater);
        Task UpdateTheater(Theater theater);
        Task DeleteTheater(int id);
    }
}
