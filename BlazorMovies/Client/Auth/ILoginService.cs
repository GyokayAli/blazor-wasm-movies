using System.Threading.Tasks;

namespace BlazorMovies.Client.Auth
{
    public interface ILoginService
    {
        Task Login(string token);
        Task Logout();
    }
}
