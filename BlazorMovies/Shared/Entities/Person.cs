using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorMovies.Shared.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public string Picture { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }
    }
}
