namespace MusicApp.Models
{
	public class AdminArtistsViewModel
	{
		public string ArtistName { get; set; } = string.Empty;
		public string? Nationality { get; set; } = string.Empty;
		public List<AdminArtistViewModel> Artists { get; set; } = new List<AdminArtistViewModel>();

	}
	public class AdminArtistViewModel
	{
		public int ArtistId { get; set; }
		public int Count { get; set; }
		public string ArtistName { get; set; } = string.Empty;
	}
	public class AdminArtistEditModel
	{
		public int ArtistId { get; set; }
		public string ArtistName { get; set; }
		public string? Nationality { get; set; } = string.Empty;
		public List<AdminSongViewModel>? Songs { get; set; }
	}
}