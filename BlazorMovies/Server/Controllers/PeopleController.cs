﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public PeopleController(ApplicationDbContext dbContext, IFileStorageService fileStorageService, IMapper mapper)
        {
            _dbContext = dbContext;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get()
        {
            return await _dbContext.People.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _dbContext.People.FirstOrDefaultAsync(x => x.Id == id);

            if (person == null) { return NotFound(); }
            return person;
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>>> GetByName(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Person>(); }
            return await _dbContext.People
                .Where(x => x.Name.Contains(searchText))
                .Take(5)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Person person)
        {
            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                byte[] personPicture = Convert.FromBase64String(person.Picture);
                person.Picture = await _fileStorageService.SaveFile(personPicture, "jpg", "people");
            }

            _dbContext.Add(person);
            await _dbContext.SaveChangesAsync();
            return person.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Person person)
        {
            var personInDb = await _dbContext.People.FirstOrDefaultAsync(x => x.Id == person.Id);

            if (personInDb == null) { return NotFound(); }

            personInDb = _mapper.Map(person, personInDb);

            try
            {
                if (!string.IsNullOrWhiteSpace(person.Picture))
                {
                    byte[] personPicture = Convert.FromBase64String(person.Picture);
                    personInDb.Picture = await _fileStorageService.EditFile(personPicture, "jpg", "people", personInDb.Picture);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}