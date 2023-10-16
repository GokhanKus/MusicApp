using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
		public IActionResult List(int? id, string q)					//id türleri q ise aranan kelimeyi temsil eder. id kısmında türe göre q kısmında aranan kelimeye göre filtreler
		{
			var searchingWord = q;										//buraya şarkı, şarkıcı ararken yazılan kelime gelir (model binding)

			var songs = _context.Songs.AsQueryable();
			
			if (id != null)												//burası şarkı türüne göre filtreleme yapacak
			{
				songs = songs
					.Include(s => s.Genres)
					.Where(s => s.Genres.Any(g => g.GenreId == id));
			}
			if (!string.IsNullOrEmpty(q))								//burası aranan kelimeye göre filtreleme yapacak
			{
				songs = songs
					.Where(s => s.Name.Contains(q) 
					|| s.Description.Contains(q));
			}
			var model = new SongsViewModel
			{
				Songs = songs.ToList()
			};

			return View("Songs",model);									//bu sekilde yaparsak List.cshtml adında dosya olusturmak yerine Songs.cshtml dosyası olusturabiliriz.
		}
	}
}
