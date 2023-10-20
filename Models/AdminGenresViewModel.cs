
using Microsoft.AspNetCore.Mvc;
using MusicApp.Entity;
using System.ComponentModel.DataAnnotations;

namespace MusicApp.Models
{
	public class AdminGenresViewModel
	{
		[Required(ErrorMessage = "GenreName field cannot be empty")]
		[StringLength(40, MinimumLength = 2, ErrorMessage = "GenreName field must bigger than 1 character")]
		public string GenreName { get; set; } = string.Empty;
		public List<AdminGenreViewModel> Genres { get; set; } = new List<AdminGenreViewModel>();
	}

	public class AdminGenreViewModel
	{
		public int GenreId { get; set; }
		public string GenreName { get; set; } = string.Empty;
		public int Count { get; set; }
	}
	public class AdminGenreEditViewModel
	{
		public int GenreId { get; set; }

		[Required(ErrorMessage = "GenreName field cannot be empty")]
		[StringLength(40, MinimumLength = 2, ErrorMessage = "GenreName field must bigger than 1 character")]
		public string GenreName { get; set; } = string.Empty;
		public List<AdminSongViewModel>? Songs { get; set; }
		public List<AdminArtistViewModel>? Artists { get; set; }
	}
}

