﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp.Data;
using MusicApp.Models;

namespace MusicApp.Controllers
{
	public class SongsController : BaseController
	{
		//private readonly SongContext _context;

		//basecontrollerdan kalıtım aldığımız için contexti injection islemini burada yapmayalım

		public SongsController(SongContext context):base(context) 
		{
			//_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult List(int? id)                    //id türleri q ise aranan kelimeyi temsil eder. id kısmında türe göre q kısmında aranan kelimeye göre filtreler
		{
			var songs = _context.Songs.AsQueryable();

			if (id != null)                                             //burası şarkı türüne göre filtreleme yapacak
			{
				songs = songs
					.Include(s => s.Genres)
					.Where(s => s.Genres.Any(g => g.GenreId == id));
			}
			
			var model = new SongsViewModel
			{
				Songs = songs.ToList()
			};

			return View("Songs", model);                                    //bu sekilde yaparsak List.cshtml adında dosya olusturmak yerine Songs.cshtml dosyasına yonlendirebiliriz.
		}
		public IActionResult Searching(string q)
		{
			var songs = _context.Songs.AsQueryable();

			if (!string.IsNullOrEmpty(q))
			{
				songs = songs
					.Include(s=>s.Artists)
					.Where(s=>s.SongName.Contains(q)||
					s.Artists.Any(a=>a.ArtistName.Contains(q)));
			}
			var model = new SongsViewModel
			{
				Songs = songs.ToList() 
			};
			return View("Songs", model);

		}
		public IActionResult Details(int id)
		{
			//var model = _context.Songs.Find(id);

			/*ONEMLİ üstteki satır varken detay sayfamıza artist bilgilerini getiremiyorduk, cünkü many to many ilişki var ve biz bu tabloyu şarkı tablomuza
			dahil etmiyorduk. aşagıdaki gibi include ederek artist bilgilerini alırız ve songs tablosu ile join yaparız artık null olarak gözükmeyecek 
			şarkının artistnamesi var ise detay sayfasında gözükecek.*/
			//aynı şekilde Genres bilgilerini de bu yüzden eklemek zorundayız.

			var model = _context.Songs
				.Include(s => s.Artists)
				.Include(g => g.Genres)
				.FirstOrDefault(s => s.SongId == id);

			//eğer 1 şarkıyı birden fazla sanatçı söylemişse detay kısmında sanatçılar alt alta çıkıyordu, bunu istemedim yan yana olsun istedim. string.Join()
			var artists = model.Artists.Select(a => a.ArtistName).ToList();
			var artistNames = string.Join(", ", artists);
			ViewBag.ArtistNames = artistNames;

			var genres = model.Genres.Select(g => g.GenreName).ToList();
			var genreNames = string.Join("&", genres);
			ViewBag.GenreNames = genreNames;

			var artistNationality = model.Artists.Select(a=>a.Nationality).ToList();

			return View(model);
		}
	}
}
