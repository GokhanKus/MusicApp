using MusicApp.Entity;

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
		public string Description { get; set; }
		public string ImageUrl { get; set; }
		public List<Genre> Genres { get; set; } //many to many relation with Genre
	}
	public class AdminCreateSongModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int[] GenreIds { get; set; }
		public DateTime ReleaseDate { get; set; } = DateTime.Now;
	}
}