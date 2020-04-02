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
