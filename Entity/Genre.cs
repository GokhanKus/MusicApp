using MusicApp.Controllers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicApp.Entity
{
    public class Genre
    {
        public Genre()
        {
            Songs = new List<Song>();
            GenreName = string.Empty;
        }
        public int GenreId { get; set; } //classismiyle aynı sonunda Id var primary key olur otomatik
        public string GenreName { get; set; }
        public List<Song> Songs { get; set; } //navigation property
    }
}
