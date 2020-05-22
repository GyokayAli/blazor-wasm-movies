using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/genres";

        public GenreRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Genre>> GetGenres(bool includeToken = true)
        {
            return await _httpService.GetHelper<List<Genre>>(url, includeToken);
        }

        public async Task<Genre> GetGenre(int id)
        {
            return await _httpService.GetHelper<Genre>($"{url}/{id}", true);
        }

        public async Task CreateGenre(Genre genre)
        {
            var response = await _httpService.Post(url, genre);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task UpdateGenre(Genre genre)
        {
            var response = await _httpService.Put(url, genre);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task DeleteGenre(int id)
        {
            await _httpService.DeleteHelper($"{url}/{id}");
        }
    }
}
