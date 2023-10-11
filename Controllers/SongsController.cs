using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
using MusicApp.Models;

namespace MusicApp.Controllers
{
	public class SongsController : Controller
	{
		private readonly SongContext _context;
        public SongsController(SongContext context)
        {
			_context = context;
        }
        public IActionResult Index()
		{
			return View();
		}
		public IActionResult List(int? id, string q)
		{
			var songs = _context.Songs;

			var model = new SongsViewModel
			{
				Songs = songs.ToList()
			};

			return View("Songs",model);			//bu sekilde yaparsak List.cshtml adında dosya olusturmak yerine Songs.cshtml dosyası olusturabiliriz.
												//view tarafından renderlanacak olan modeli olusturalım
		}
	}
}
