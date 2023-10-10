using MusicApp.Controllers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicApp.Entity
{
    public class Genre
    {
        public int GenreId { get; set; } //classismiyle aynı sonunda Id var primary key olur otomatik
        public string Name { get; set; }
        public List<Song> Songs { get; set; } //navigation property
    }
}
