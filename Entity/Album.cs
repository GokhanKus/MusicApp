namespace MusicApp.Entity
{
	public class Album
	{
        public Album()
        {
            Songs = new List<Song>(); //bu ctoru tanımlamayınca album bilgisini eklerken song kısmında null deger ataması hatası alıyoruz?
            AlbumName = string.Empty;
        }
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public List<Song> Songs { get; set; }
    }
}
