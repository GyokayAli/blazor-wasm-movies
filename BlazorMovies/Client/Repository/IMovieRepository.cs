using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IMovieRepository
    {
        Task<IndexPageDTO> GetIndexPageDTO();
        Task<MovieDetailsDTO> GetMovieDetailsDTO(int id);
        Task<int> CreateMovie (Movie movie);
    }
}
