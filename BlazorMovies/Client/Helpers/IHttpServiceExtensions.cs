using BlazorMovies.Shared.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Helpers
{
    public static class IHttpServiceExtensions
    {
        public static async Task<T> GetHelper<T>(this IHttpService httpService, string url)
        {
            var response = await httpService.Get<T>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public static async Task<PaginatedResponse<T>> GetHelper<T>(this IHttpService httpService, string url, PaginationDTO pagination)
        {
            string newURL = string.Empty;
            if (url.Contains("?"))
            {
                newURL = $"{url}&page={pagination.Page}&recordsPerPage={pagination.RecordsPerPage}";
            }
            else
            {
                newURL = $"{url}?page={pagination.Page}&recordsPerPage={pagination.RecordsPerPage}";
            }

            var httpResponse = await httpService.Get<T>(newURL);
            var totalAmountPages = int.Parse(httpResponse.HttpResponseMessage.Headers.GetValues("totalAmountPages").FirstOrDefault());
            var paginatedResponse = new PaginatedResponse<T>
            {
                Response = httpResponse.Response,
                TotalAmountPages = totalAmountPages
            };

            return paginatedResponse;
        }

        public static async Task DeleteHelper(this IHttpService httpService, string url)
        {
            var response = await httpService.Delete(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
