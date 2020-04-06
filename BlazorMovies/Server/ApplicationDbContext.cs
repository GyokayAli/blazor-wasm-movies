using BlazorMovies.Shared.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BlazorMovies.Server
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoviesActors>().HasKey(x => new { x.MovieId, x.PersonId });
            modelBuilder.Entity<MoviesGenres>().HasKey(x => new { x.MovieId, x.GenreId });

            //var userAdminId = "9307c77f-7de2-4f4b-b836-7f605b356c57";
            //var emailAdmin = "gyokay@dev.com";

            //var hasher = new PasswordHasher<IdentityUser>();
            //var userAdmin = new IdentityUser()
            //{
            //    Id = userAdminId,
            //    Email = emailAdmin,
            //    UserName = emailAdmin,
            //    NormalizedEmail = emailAdmin,
            //    NormalizedUserName = emailAdmin,
            //    EmailConfirmed = true,
            //    PasswordHash = hasher.HashPassword(null, "aA123456!")
            //};

            //modelBuilder.Entity<IdentityUser>().HasData(userAdmin);

            //modelBuilder.Entity<IdentityUserClaim<string>>()
            //    .HasData(new IdentityUserClaim<string>()
            //    {
            //        Id = 1,
            //        ClaimType = ClaimTypes.Role,
            //        ClaimValue = "Admin",
            //        UserId = userAdminId
            //    });

            //// Add Genres
            //modelBuilder.Entity<Genre>()
            //    .HasData(
            //        new Genre { Id = 1, Name = "Comedy" },
            //        new Genre { Id = 2, Name = "Action" },
            //        new Genre { Id = 3, Name = "Horror" },
            //        new Genre { Id = 4, Name = "Romance" },
            //        new Genre { Id = 5, Name = "Thriller" },
            //        new Genre { Id = 6, Name = "Drama" },
            //        new Genre { Id = 7, Name = "Mystery" },
            //        new Genre { Id = 8, Name = "Crime" },
            //        new Genre { Id = 9, Name = "Animation" },
            //        new Genre { Id = 10, Name = "Fantasy" },
            //        new Genre { Id = 11, Name = "Comedy-Romance" },
            //        new Genre { Id = 12, Name = "Action-Comedy" },
            //        new Genre { Id = 13, Name = "Superhero" },
            //        new Genre { Id = 14, Name = "Sci-Fi" },
            //        new Genre { Id = 15, Name = "Biography" },
            //        new Genre { Id = 16, Name = "Documentary" },
            //        new Genre { Id = 17, Name = "Musical" },
            //        new Genre { Id = 18, Name = "Family" },
            //        new Genre { Id = 19, Name = "War" },
            //        new Genre { Id = 20, Name = "Western" }
            //    );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MovieRating> MovieRatings { get; set; }
    }
}
