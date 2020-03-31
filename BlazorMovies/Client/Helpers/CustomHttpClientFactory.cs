using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Helpers
{
    public class CustomHttpClientFactory
    {
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _authenticationService;
        private readonly NavigationManager _navigationManager;
        private readonly string authorizationHeader = "Authorization";

        public CustomHttpClientFactory(HttpClient httpClient, IAccessTokenProvider authenticationService,
            NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;
            _navigationManager = navigationManager;
        }

        public async Task<HttpClient> GetHttpClientWithToken()
        {
            if (!_httpClient.DefaultRequestHeaders.Contains(authorizationHeader))
            {
                var tokenResult = await _authenticationService.RequestAccessToken();

                if (tokenResult.TryGetToken(out var token))
                {
                    _httpClient.DefaultRequestHeaders.Add(authorizationHeader, $"Bearer {token.Value}");
                }
                else
                {
                    _navigationManager.NavigateTo(tokenResult.RedirectUrl);
                }
            }

            return _httpClient;
        }

        public HttpClient GetHttpClientWithoutToken()
        {
            if (_httpClient.DefaultRequestHeaders.Contains(authorizationHeader))
            {
                _httpClient.DefaultRequestHeaders.Remove(authorizationHeader);
            }

            return _httpClient;
        }
    }
}
