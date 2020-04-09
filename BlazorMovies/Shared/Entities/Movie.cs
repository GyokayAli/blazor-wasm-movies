using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorMovies.Shared.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public bool InTheaters { get; set; }
        public string Trailer { get; set; }
        [Required]
        public DateTime? ReleaseDate { get; set; }
        public string Poster { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; }
        public List<MoviesActors> MoviesActors { get; set; }
        public List<MoviesTheaters> MoviesTheaters { get; set; }

        public string TitleBrief
        {
            get
            {
                if (string.IsNullOrEmpty(Title))
                {
                    return null;
                }

                if (Title.Length > 60)
                {
                    return Title.Substring(0, 60) + "...";
                }
                else
                {
                    return Title;
                }
            }
        }

        public Movie()
        {
            MoviesGenres = new List<MoviesGenres>();
            MoviesActors = new List<MoviesActors>();
            MoviesTheaters = new List<MoviesTheaters>();
        }
    }
}
