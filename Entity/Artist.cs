namespace MusicApp.Entity
{
	public class Artist
	{
        public Artist()
        {
            Songs= new List<Song>();
            ArtistName = string.Empty;
        }
        public int ArtistId { get; set; }
        
        public string ArtistName{ get; set; }
        
        public string? Nationality{ get; set; }
        public List<Song> Songs { get; set; }
    }
}
