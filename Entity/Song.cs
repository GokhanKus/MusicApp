namespace MusicApp.Entity
{
    public class Song
    {
        public Song()
        {
            Genres = new List<Genre>(); //bu ctoru tanımlamayınca şarkıyı eklerken tür kısmında null deger ataması hatası alıyoruz
            Albums = new List<Album>();
            Artists = new List<Artist>();

            SongName = string.Empty;
        }
        public int SongId { get; set; }
        public string SongName { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; } 
        public string? Language { get; set; }
        public List<Genre> Genres { get; set; } //many to many relation with Genre
        public List<Album> Albums { get; set; } //many to many relation with Album ( burada one to many de yapılabilir )
        //bir albumde birden fazla şarkı olabilir, bir şarkı da birden fazla albumde bulunabilir(aynı şarkıyı farklı biri de söylemis olabilir.)
        public List<Artist> Artists { get; set; } //many to many bir şarkıyı birden fazla sanatçı soyler, bir sanatçı da birden fazla şarkı soyler

        public int? ReleaseDate { get; set; }
    }
}
