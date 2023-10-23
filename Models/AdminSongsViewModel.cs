using MusicApp.Entity;
using MusicApp.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicApp.Models
{
	public class AdminSongsViewModel
	{
		public List<AdminSongViewModel> Songs { get; set; } = new List<AdminSongViewModel>();
	}
	public class AdminSongViewModel
	{
		public int SongId { get; set; }
		public string SongName { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? ImageUrl { get; set; }
		public List<Genre> Genres { get; set; } = new List<Genre>(); //many to many relation with Genre
		public int? ReleaseDate { get; set; }
	}
	public class AdminCreateSongModel
	{
		[Required(ErrorMessage = "Name field cannot be empty")]
		[StringLength(80, ErrorMessage = "Max 80 character allowed")]
		public string SongName { get; set; } = string.Empty;

		//Description field null olabilir, ancak değer girilmek isteniyorsa 10-800 karakter arasında değer girilmelidir.
		[StringLength(800, MinimumLength = 10, ErrorMessage = "Description field: Min: 10, Max: 800 character allowed")]
		public string? Description { get; set; }

		[Required (ErrorMessage = "You must choose at least one genre")]
		public int[] GenreIds { get; set; }

		public string ArtistName { get; set; } = string.Empty;

        //[ReleaseDate(ErrorMessage = "Release year must be between 1800 and current year.")]
        [ReleaseDateRange(1800,ErrorMessage = $"Release year must be between 1800 and current year.")]
		public int? ReleaseDate { get; set; }
        public string? Language { get; set; }

    }
	public class AdminEditSongModel
	{
		public int SongId { get; set; }

		[Required(ErrorMessage = "Name field cannot be empty")]
		[StringLength(80, ErrorMessage = "Max 80 character allowed")]
		public string SongName { get; set; } = string.Empty;

		//Description field null olabilir, ancak değer girilmek isteniyorsa 10-800 karakter arasında değer girilmelidir.
		[StringLength(800, MinimumLength = 10, ErrorMessage = "Description field: Min: 10, Max: 800 character allowed")]
		public string? Description { get; set; }

		[Required(ErrorMessage = "You must choose at least one genre")]
		public int[] GenreIds { get; set; }
		public List<string> ArtistNames { get; set;} = new List<string>();
		public string ArtistNamesString { get; set; }

		[ReleaseDateRange(1800, ErrorMessage = $"Release year must be between 1800 and current year.")]
		public int? ReleaseDate { get; set; }

		public string? ImageUrl { get; set; }
	}
}