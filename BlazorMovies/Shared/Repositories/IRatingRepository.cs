using BlazorMovies.Shared.Entities;
using System.Threading.Tasks;

namespace BlazorMovies.Shared.Repositories
{
    public interface IRatingRepository
    {
        Task Vote(MovieRating movieRating);
    }
}
