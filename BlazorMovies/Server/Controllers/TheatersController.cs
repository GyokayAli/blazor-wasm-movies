using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class TheatersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public TheatersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Theater>>> Get()
        {
            return await _dbContext.Theaters.ToListAsync();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<TheaterDetailsDTO>> Get(int id)
        {
            var theater = await _dbContext.Theaters.Where(x => x.Id == id)
                .Include(x => x.MoviesTheaters).ThenInclude(x => x.Movie)
                .FirstOrDefaultAsync();

            if (theater == null) { return NotFound(); }

            var model = new TheaterDetailsDTO { Theater = theater };
            model.Movies = theater.MoviesTheaters.Select(x =>
                new Movie
                {
                    Title = x.Movie.Title,
                    Poster = x.Movie.Poster,
                    Trailer = x.Movie.Trailer,
                    ReleaseDate = x.Movie.ReleaseDate,
                    Summary = x.Movie.Summary,
                    Id = x.MovieId
                }).ToList();

            model.Movies = model.Movies.OrderByDescending(x => x.ReleaseDate).ToList();

            return model;
        }

        [HttpGet("search/{keyword}")]
        public async Task<ActionResult<List<Theater>>> GetByKeyword(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) { return new List<Theater>(); }
            return await _dbContext.Theaters
                .Where(x => x.Name.Contains(keyword.Trim()) || x.Address.Contains(keyword.Trim()))
                .Take(5)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Theater theater)
        {
            _dbContext.Add(theater);
            await _dbContext.SaveChangesAsync();
            return theater.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Theater theater)
        {
            _dbContext.Attach(theater).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var theater = await _dbContext.Theaters.FirstOrDefaultAsync(x => x.Id == id);
            if (theater == null)
            {
                return NotFound();
            }

            _dbContext.Remove(theater);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}