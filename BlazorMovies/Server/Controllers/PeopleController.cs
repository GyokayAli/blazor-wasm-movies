using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PeopleController : ControllerBase
    {
        #region "Fields"

        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        #endregion

        #region "Constructor"

        public PeopleController(ApplicationDbContext dbContext, IFileStorageService fileStorageService, IMapper mapper)
        {
            _dbContext = dbContext;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }
        #endregion

        #region "GET Methods"
        /// <summary>
        /// Gets a paginated result of People.
        /// </summary>
        /// <param name="pagination">The pagination options.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _dbContext.People.AsQueryable();
            await HttpContext.InsertPaginationParamatersInResponse(queryable, pagination.RecordsPerPage);

            return await queryable.Paginate(pagination).ToListAsync();
        }

        /// <summary>
        /// Gets a Person by Id to edit.
        /// </summary>
        /// <param name="id">The Person Id.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _dbContext.People.FirstOrDefaultAsync(x => x.Id == id);

            if (person == null) { return NotFound(); }
            return person;
        }

        /// <summary>
        /// Gets a Person by Id to display.
        /// </summary>
        /// <param name="id">The Person Id.</param>
        /// <returns></returns>
        [HttpGet("view/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PersonDetailsDTO>> GetDisplay(int id)
        {
            var person = await _dbContext.People.Where(x => x.Id == id)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Movie)
                .FirstOrDefaultAsync();

            if (person == null) { return NotFound(); }

            var model = new PersonDetailsDTO();
            person.MoviesActors = person.MoviesActors.OrderByDescending(x => x.Movie.ReleaseDate).ToList();

            if (person.MoviesActors.Any())
            {
                var featuredMovie = person.MoviesActors.FirstOrDefault();
                if (featuredMovie != null)
                {
                    model.FeaturedMovieTrailer = featuredMovie.Movie.Trailer;
                }
            }

            model.Person = person;
            model.Movies = person.MoviesActors.Select(x =>
                new Movie
                {
                    Id = x.MovieId,
                    Poster = x.Movie.Poster,
                    Title = x.Movie.Title,
                    ReleaseDate = x.Movie.ReleaseDate
                }).ToList();

            return model;
        }

        /// <summary>
        /// Gets the result of People matching the search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>>> GetByName(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Person>(); }
            return await _dbContext.People
                .Where(x => x.Name.Contains(searchText))
                .Take(5)
                .ToListAsync();
        }
        #endregion

        #region "POST Methods"

        /// <summary>
        /// Creates a new Person.
        /// </summary>
        /// <param name="person">The Person to be created.</param>
        /// <returns></returns>
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
        #endregion

        #region "PUT Methods"

        /// <summary>
        /// Updates a Person.
        /// </summary>
        /// <param name="person">The Person to be updated.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Put(Person person)
        {
            var personInDb = await _dbContext.People.FirstOrDefaultAsync(x => x.Id == person.Id);

            if (personInDb == null) { return NotFound(); }

            personInDb = _mapper.Map(person, personInDb);

            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                byte[] personPicture = Convert.FromBase64String(person.Picture);
                personInDb.Picture = await _fileStorageService.EditFile(personPicture, "jpg", "people", personInDb.Picture);
            }

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region "DELETE Methods"

        /// <summary>
        /// Deletes a Person.
        /// </summary>
        /// <param name="id">The Person Id.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var person = await _dbContext.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            _dbContext.Remove(person);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}