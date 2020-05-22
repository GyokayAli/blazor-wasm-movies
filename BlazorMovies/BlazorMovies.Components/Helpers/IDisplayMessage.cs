using System.Threading.Tasks;

namespace BlazorMovies.Components.Helpers
{
    public interface IDisplayMessage
    {
        ValueTask DisplayErrorMessage(string message);
        ValueTask DisplaySuccessMessage(string message);
    }
}
