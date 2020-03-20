using System;

namespace BlazorMovies.Shared.Entities
{
    public class Movie
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
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
    }
}
