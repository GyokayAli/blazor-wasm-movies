using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Shared.Repositories
{
    public interface ITheaterRepository
    {
        Task<List<Theater>> GetTheaters();
        Task<TheaterDetailsDTO> GetTheater(int id);
        Task CreateTheater(Theater theater);
        Task UpdateTheater(Theater theater);
        Task DeleteTheater(int id);
        Task<List<Theater>> GetTheaterByKeyword(string keyword);
    }
}
