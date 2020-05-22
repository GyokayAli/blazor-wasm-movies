using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/people";

        public PersonRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<PaginatedResponse<List<Person>>> GetPeople(PaginationDTO pagination)
        {
            return await _httpService.GetHelper<List<Person>>(url, pagination);
        }

        public async Task<List<Person>> GetPeopleByName(string name)
        {
            return await _httpService.GetHelper<List<Person>>($"{url}/search/{name}");
        }

        public async Task CreatePerson(Person person)
        {
            var response = await _httpService.Post(url, person);

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task UpdatePerson(Person person)
        {
            var response = await _httpService.Put(url, person);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task<Person> GetPersonById(int id)
        {
            return await _httpService.GetHelper<Person>($"{url}/{id}");
        }

        public async Task<PersonDetailsDTO> GetPersonDetailsDTO(int id)
        {
            return await _httpService.GetHelper<PersonDetailsDTO>($"{url}/view/{id}", false);
        }

        public async Task DeletePerson(int id)
        {
            await _httpService.DeleteHelper($"{url}/{id}");
        }
    }
}
