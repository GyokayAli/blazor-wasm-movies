using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.Entities;
using System;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly IHttpService _httpService;
        private readonly string urlBase = "api/rating";

        public RatingRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task Vote(MovieRating movieRating)
        {
            var httpResponse = await _httpService.Post(urlBase, movieRating);

            if (!httpResponse.Success)
            {
                throw new ApplicationException(await httpResponse.GetBody());
            }
        }
    }
}
