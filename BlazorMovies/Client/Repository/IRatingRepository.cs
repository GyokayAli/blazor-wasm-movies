using BlazorMovies.Shared.Entities;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IRatingRepository
    {
        Task Vote(MovieRating movieRating);
    }
}
