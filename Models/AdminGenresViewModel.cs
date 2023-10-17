
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MusicApp.Models
{
	public class AdminGenresViewModel
	{
        [Required(ErrorMessage = "Name field cannot be empty")]
		[StringLength(40, MinimumLength = 2, ErrorMessage = "Name field must bigger than 2 character")]
		public string Name { get; set; }
        public List<AdminGenreViewModel> Genres { get; set; }
	}
	public class AdminGenreViewModel
	{
        public int GenreId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }	
	}
	public class AdminGenreEditViewModel
	{
        public int GenreId { get; set; }
        public string Name { get; set; }
        public List<AdminSongViewModel> Songs { get; set; }
    }
}
