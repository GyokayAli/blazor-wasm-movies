using BlazorMovies.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IPersonRepository
    {
        Task<List<Person>> GetPeople();
        Task<List<Person>> GetPeopleByName(string name);
        Task CreatePerson(Person person);
    }
}
