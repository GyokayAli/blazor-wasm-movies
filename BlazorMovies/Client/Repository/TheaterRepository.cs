using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class TheaterRepository : ITheaterRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/theaters";

        public TheaterRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Theater>> GetTheaters()
        {
            return await _httpService.GetHelper<List<Theater>>(url, false);
        }

        public async Task<Theater> GetTheater(int id)
        {
            return await _httpService.GetHelper<Theater>($"{url}/{id}", false);
        }

        public async Task<List<Theater>> GetTheaterByKeyword(string keyword)
        {
            return await _httpService.GetHelper<List<Theater>>($"{url}/search/{keyword}");
        }

        public async Task CreateTheater(Theater theater)
        {
            var response = await _httpService.Post(url, theater);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task UpdateTheater(Theater theater)
        {
            var response = await _httpService.Put(url, theater);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task DeleteTheater(int id)
        {
            await _httpService.DeleteHelper($"{url}/{id}");
        }
    }
}
