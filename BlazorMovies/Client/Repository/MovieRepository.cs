using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IHttpService _httpService;
        private string url = "api/movies";

        public MovieRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IndexPageDTO> GetIndexPageDTO()
        {
            return await _httpService.GetHelper<IndexPageDTO>(url);
        }

        public async Task<MovieDetailsDTO> GetMovieDetailsDTO(int id)
        {
            return await _httpService.GetHelper<MovieDetailsDTO>($"{url}/{id}");
        }
        public async Task<MovieUpdateDTO> GetMovieForUpdate(int id)
        {
            return await _httpService.GetHelper<MovieUpdateDTO>($"{url}/update/{id}");
        }

        public async Task<int> CreateMovie(Movie movie)
        {
            var response = await _httpService.Post<Movie, int>(url, movie);

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response.Response;
        }

        public async Task UpdateMovie(Movie movie)
        {
            var response = await _httpService.Put(url, movie);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
