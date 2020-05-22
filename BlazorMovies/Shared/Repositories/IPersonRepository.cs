﻿using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Shared.Repositories
{
    public interface IPersonRepository
    {
        Task<PaginatedResponse<List<Person>>> GetPeople(PaginationDTO pagination);
        Task<List<Person>> GetPeopleByName(string name);
        Task<Person> GetPersonById(int id);
        Task<PersonDetailsDTO> GetPersonDetailsDTO(int id);
        Task CreatePerson(Person person);
        Task UpdatePerson(Person person);
        Task DeletePerson(int id);
    }
}
