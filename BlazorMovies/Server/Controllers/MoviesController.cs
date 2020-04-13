using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MoviesController : ControllerBase
    {
        #region "Fields"

        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private const string ContainerName = "movies";
        #endregion

        #region "Constructor"

        public MoviesController(ApplicationDbContext dbContext, IFileStorageService fileStorageService,
            IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
            _userManager = userManager;
        }
        #endregion

        #region "GET Methods"

        /// <summary>
        /// Gets the latest 6 Movies in theaters.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IndexPageDTO>> Get()
        {
            int limit = 6;

            var moviesInTheaters = await _dbContext.Movies
                .Where(x => x.InTheaters).Take(limit)
                .OrderByDescending(x => x.ReleaseDate)
                .ToListAsync();

            var todaysDate = DateTime.Today;

            var upcomingReleases = await _dbContext.Movies
                .Where(x => x.ReleaseDate > todaysDate)
                .OrderBy(x => x.ReleaseDate).Take(limit)
                .ToListAsync();

            var response = new IndexPageDTO()
            {
                InTheaters = moviesInTheaters,
                UpcomingReleases = upcomingReleases
            };

            return response;
        }

        /// <summary>
        /// Gets a Movie by Id.
        /// </summary>
        /// <param name="id">The Movie Id.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<MovieDetailsDTO>> Get(int id)
        {
            var movie = await _dbContext.Movies.Where(x => x.Id == id)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Person)
                .Include(x => x.MoviesTheaters).ThenInclude(x => x.Theater)
                .FirstOrDefaultAsync();

            if (movie == null) { return NotFound(); }

            var averageVote = 0.0;
            var userVote = 0;

            if (await _dbContext.MovieRatings.AnyAsync(x => x.MovieId == id))
            {
                averageVote = await _dbContext.MovieRatings.Where(x => x.MovieId == id)
                    .AverageAsync(x => x.Rate);

                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                    var userId = user.Id;

                    var userVoteInDb = await _dbContext.MovieRatings
                        .FirstOrDefaultAsync(x => x.MovieId == id && x.UserId == userId);

                    if (userVoteInDb != null)
                    {
                        userVote = userVoteInDb.Rate;
                    }
                }
            }

            movie.MoviesActors = movie.MoviesActors.OrderBy(x => x.Order).ToList();

            var model = new MovieDetailsDTO
            {
                Movie = movie,
                UserVote = userVote,
                AverageVote = averageVote,
                Genres = movie.MoviesGenres.Select(x => x.Genre).ToList()
            };

            model.Actors = movie.MoviesActors.Select(x =>
                new Person
                {
                    Name = x.Person.Name,
                    Picture = x.Person.Picture,
                    Character = x.Character,
                    Id = x.PersonId
                }).ToList();

            model.Theaters = movie.MoviesTheaters.Select(x =>
                new Theater
                {
                    Name = x.Theater.Name,
                    Address = x.Theater.Address,
                    Id = x.TheaterId
                }).ToList();

            return model;
        }

        /// <summary>
        /// Gets a Movie by Id for update.
        /// </summary>
        /// <param name="id">The Movie Id.</param>
        /// <returns></returns>
        [HttpGet("update/{id}")]
        public async Task<ActionResult<MovieUpdateDTO>> PutGet(int id)
        {
            var movieActionResult = await Get(id);
            if (movieActionResult == null) { return NotFound(); }

            var movieDetailDTO = movieActionResult.Value;
            var selectedGenreIds = movieDetailDTO.Genres.Select(x => x.Id).ToList();
            var notSelectedGenres = await _dbContext.Genres
                .Where(x => !selectedGenreIds.Contains(x.Id))
                .ToListAsync();

            var model = new MovieUpdateDTO()
            {
                Movie = movieDetailDTO.Movie,
                SelectedGenres = movieDetailDTO.Genres,
                NotSelectedGenres = notSelectedGenres,
                Actors = movieDetailDTO.Actors,
                Theaters = movieDetailDTO.Theaters
            };

            return model;
        }
        #endregion

        #region "POST Methods"

        /// <summary>
        /// Gets the filtered out result of Movies.
        /// </summary>
        /// <param name="filterMoviesDto">The applied filters.</param>
        /// <returns></returns>
        [HttpPost("filter")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Movie>>> Filter(FilterMoviesDTO filterMoviesDto)
        {
            var moviesQueryable = _dbContext.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterMoviesDto.Title))
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.Title.Contains(filterMoviesDto.Title));
            }

            if (filterMoviesDto.InTheaters)
            {
                moviesQueryable = moviesQueryable.Where(x => x.InTheaters);
            }

            if (filterMoviesDto.UpcomingReleases)
            {
                var today = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(x => x.ReleaseDate > today);
            }

            if (filterMoviesDto.OrderByVotes)
            {
                var movieRatings = _dbContext.MovieRatings.AsQueryable();

                moviesQueryable = moviesQueryable.OrderBy(x =>
                    movieRatings.Where(y => y.MovieId == x.Id).Average(z => z.Rate)
                );
            }

            if (filterMoviesDto.GenreId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.MoviesGenres.Select(y => y.GenreId)
                    .Contains(filterMoviesDto.GenreId));
            }

            await HttpContext.InsertPaginationParamatersInResponse(moviesQueryable, filterMoviesDto.RecordsPerPage);

            var movies = await moviesQueryable.Paginate(filterMoviesDto.Pagination).ToListAsync();

            return movies;
        }

        /// <summary>
        /// Creates a new Movie.
        /// </summary>
        /// <param name="movie">The Movie to be created.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Post(Movie movie)
        {
            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                byte[] poster = Convert.FromBase64String(movie.Poster);
                movie.Poster = await _fileStorageService.SaveFile(poster, "jpg", ContainerName);
            }

            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            _dbContext.Add(movie);
            await _dbContext.SaveChangesAsync();
            return movie.Id;
        }
        #endregion

        #region "PUT Methods"

        /// <summary>
        /// Updates a Movies.
        /// </summary>
        /// <param name="movie">The Movie to be updated.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Put(Movie movie)
        {
            var movieInDb = await _dbContext.Movies.FirstOrDefaultAsync(x => x.Id == movie.Id);

            if (movieInDb == null) { return NotFound(); }

            movieInDb = _mapper.Map(movie, movieInDb);

            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                byte[] moviePoster = Convert.FromBase64String(movie.Poster);
                movieInDb.Poster = await _fileStorageService.EditFile(moviePoster, "jpg", ContainerName, movieInDb.Poster);
            }

            await _dbContext.Database.ExecuteSqlInterpolatedAsync($"delete from MoviesActors where MovieId = {movie.Id}; delete from MoviesGenres where MovieId = {movie.Id}; delete from MoviesTheaters where MovieId = {movie.Id};");

            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            movieInDb.MoviesActors = movie.MoviesActors;
            movieInDb.MoviesGenres = movie.MoviesGenres;
            movieInDb.MoviesTheaters = movie.MoviesTheaters;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region "DELETE Methods"

        /// <summary>
        /// Deletes a Movie.
        /// </summary>
        /// <param name="id">The Movie Id.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            _dbContext.Remove(movie);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}