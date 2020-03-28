using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private const string ContainerName = "movies";

        public MoviesController(ApplicationDbContext dbContext, IFileStorageService fileStorageService, IMapper mapper)
        {
            _dbContext = dbContext;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IndexPageDTO>> Get()
        {
            var limit = 6;

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

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetailsDTO>> Get(int id)
        {
            var movie = await _dbContext.Movies.Where(x => x.Id == id)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Person)
                .FirstOrDefaultAsync();

            if (movie == null) { return NotFound(); }

            movie.MoviesActors = movie.MoviesActors.OrderBy(x => x.Order).ToList();

            var model = new MovieDetailsDTO();
            model.Movie = movie;
            model.Genres = movie.MoviesGenres.Select(x => x.Genre).ToList();
            model.Actors = movie.MoviesActors.Select(x =>
                new Person
                {
                    Name = x.Person.Name,
                    Picture = x.Person.Picture,
                    Character = x.Character,
                    Id = x.PersonId
                }).ToList();

            return model;
        }

        [HttpPost("filter")]
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
                Actors = movieDetailDTO.Actors
            };

            return model;
        }

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

            await _dbContext.Database.ExecuteSqlInterpolatedAsync($"delete from MoviesActors where MovieId = {movie.Id}; delete from MoviesGenres where MovieId = {movie.Id};");

            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            movieInDb.MoviesActors = movie.MoviesActors;
            movieInDb.MoviesGenres = movie.MoviesGenres;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

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
    }
}