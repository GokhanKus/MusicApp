using MusicApp.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicApp.Models
{
	public class AdminSongsViewModel
	{
		public List<AdminSongViewModel> Songs { get; set; }
	}
	public class AdminSongViewModel
	{
		public int SongId { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public string? ImageUrl { get; set; }
		public List<Genre> Genres { get; set; } //many to many relation with Genre
		public int? ReleaseDate { get; set; }
	}
	public class AdminCreateSongModel
	{
		[Required (ErrorMessage = "Name field cannot be empty")] 
		[StringLength(80, ErrorMessage = "Max 80 character allowed")]
		public string Name { get; set; }

		//Description field null olabilir, ancak değer girilmek isteniyorsa 10-800 karakter arasında değer girilmelidir.
		[StringLength(800, MinimumLength = 10, ErrorMessage = "Description field: Min: 10, Max: 800 character allowed")]
		public string? Description { get; set; }

		[Required (ErrorMessage = "You must choose at least one genre")]
		public int[] GenreIds { get; set; }

		public int? ReleaseDate { get; set; } 

	}
	public class AdminEditSongModel
	{
		public int SongId { get; set; }

		[Required(ErrorMessage = "Name field cannot be empty")]
		[StringLength(80, ErrorMessage = "Max 80 character allowed")]
		public string Name { get; set; }

		//Description field null olabilir, ancak değer girilmek isteniyorsa 10-800 karakter arasında değer girilmelidir.
		[StringLength(800, MinimumLength = 10, ErrorMessage = "Description field: Min: 10, Max: 800 character allowed")]
		public string? Description { get; set; }

		[Required(ErrorMessage = "You must choose at least one genre")]
		public int[] GenreIds { get; set; }
		public int? ReleaseDate { get; set; }
		public string? ImageUrl { get; set; }
	}
}