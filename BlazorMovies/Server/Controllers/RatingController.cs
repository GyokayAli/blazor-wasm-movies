using System;
using System.Threading.Tasks;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public RatingController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Rate(MovieRating movieRating)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var userId = user.Id;

            var currentRating = await _dbContext.MovieRatings
                .FirstOrDefaultAsync(x => x.MovieId == movieRating.MovieId && x.UserId == userId);

            if (currentRating == null)
            {
                movieRating.UserId = userId;
                movieRating.RatingDate = DateTime.Today;
                _dbContext.Add(movieRating);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                currentRating.Rate = movieRating.Rate;
                await _dbContext.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}