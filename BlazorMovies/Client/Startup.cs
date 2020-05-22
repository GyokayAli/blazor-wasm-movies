using Blazor.FileReader;
using BlazorMovies.Client.Helpers;
using BlazorMovies.Client.Repository;
using BlazorMovies.Components.Helpers;
using BlazorMovies.Shared.Repositories;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorMovies.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IDisplayMessage, DisplayMessage>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITheaterRepository, TheaterRepository>();
            services.AddTransient<CustomHttpClientFactory>();

            services.AddFileReaderService(options => options.InitializeOnFirstCall = true);

            services.AddApiAuthorization();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
