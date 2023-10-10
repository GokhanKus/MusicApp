namespace MusicApp.Entity
{
    public class Song
    {
        public int SongId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<Genre> Genres { get; set; } //many to many relation with Genre
    }
}
