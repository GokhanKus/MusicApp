using Microsoft.AspNetCore.Mvc;
using MusicApp.Entity;

namespace MusicApp.Models
{
    public class HomePageViewModel :Controller
    {
        public List<Song> PopularSongs { get; set; }
    }
}
