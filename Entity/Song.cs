namespace MusicApp.Entity
{
    public class Song
    {
        public Song()
        {
            Genres = new List<Genre>(); //bu ctoru tanımlamayınca şarkıyı eklerken tür kısmında null deger ataması hatası alıyoruz
            Name = string.Empty;
        }
        public int SongId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<Genre> Genres { get; set; } //many to many relation with Genre
        public int? ReleaseDate { get; set; }
    }
}
