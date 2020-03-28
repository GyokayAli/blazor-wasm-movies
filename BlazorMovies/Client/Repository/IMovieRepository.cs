using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IMovieRepository
    {
        Task<IndexPageDTO> GetIndexPageDTO();
        Task<MovieDetailsDTO> GetMovieDetailsDTO(int id);
        Task<int> CreateMovie (Movie movie);
        Task<MovieUpdateDTO> GetMovieForUpdate(int id);
        Task UpdateMovie(Movie movie);
        Task DeleteMovie(int id);
        Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMoviesDTO filterMoviesDto);
    }
}
