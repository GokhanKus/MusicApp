using MusicApp.Entity;

namespace MusicApp.Models
{
	public class AdminGenresViewModel
	{
        public string Name { get; set; }
        public List<AdminGenreViewModel> Genres { get; set; }
	}
	public class AdminGenreViewModel
	{
        public int GenreId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }	
	}
}
