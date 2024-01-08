using Microsoft.AspNetCore.Mvc;
using MusicApp.Entity;
using X.PagedList;

namespace MusicApp.Models
{
    public class HomePageViewModel 
    {
        //public List<Song> PopularSongs { get; set; }

		public IPagedList<Song> PopularSongs { get; set; }

        public int s { get; set; }



	}
}
