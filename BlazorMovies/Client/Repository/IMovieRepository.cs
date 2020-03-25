using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IMovieRepository
    {
        Task<IndexPageDTO> GetIndexPageDTO();
        Task<int> CreateMovie (Movie movie);
    }
}
