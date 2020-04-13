using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class GenresController : ControllerBase
    {
        #region "Fields"

        private readonly ApplicationDbContext _dbContext;
        #endregion

        #region "Constructor"

        public GenresController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region "GET Methods"

        /// <summary>
        /// Gets all Genres.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            return await _dbContext.Genres.ToListAsync();
        }

        /// <summary>
        /// Gets a Genre by Id.
        /// </summary>
        /// <param name="id">The Genre Id.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> Get(int id)
        {
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null) { return NotFound(); }
            return genre;
        }
        #endregion

        #region "POST Methods"

        /// <summary>
        /// Creates a new Genre
        /// </summary>
        /// <param name="genre">The Genre to be created.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Post(Genre genre)
        {
            _dbContext.Add(genre);
            await _dbContext.SaveChangesAsync();
            return genre.Id;
        }
        #endregion

        #region "PUT Methods"

        /// <summary>
        /// Updates a Genre.
        /// </summary>
        /// <param name="genre">The Genre to be updated.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Put(Genre genre)
        {
            _dbContext.Attach(genre).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region "DELETE Methods"


        /// <summary>
        /// Delete a Genre.
        /// </summary>
        /// <param name="id">The Genre Id.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            _dbContext.Remove(genre);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}