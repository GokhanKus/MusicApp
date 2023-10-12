using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp.Data;
using MusicApp.Entity;
using MusicApp.Models;

namespace MusicApp.Controllers
{
	public class AdminController : Controller
	{
		private readonly SongContext _context;
		public AdminController(SongContext context)
		{
			_context = context; //inject işlemi
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult AddSong()
		{
			ViewBag.Genres = _context.Genres.ToList();
			return View(new AdminCreateSongModel());
		}

		[HttpPost]
		public IActionResult AddSong(AdminCreateSongModel model) //model bilgilerini sayfaya post edeceğiz
		{
			//if (model.Name == null)
			//{
			//	ModelState.AddModelError("", "Please enter song's name");
			//}

			//if (ModelState.IsValid)
			//{

			//}
			var entity = new Song
			{
				Name = model.Name,
				Description = model.Description,
				ImageUrl = "No_Image.jpg"
			};
			foreach (var id in model.GenreIds)
			{
				entity.Genres.Add(_context.Genres.FirstOrDefault(i => i.GenreId == id));
			}
			_context.Songs.Add(entity);
			_context.SaveChanges();

			//return RedirectToAction("SongList");
			return View();
		}
		public IActionResult SongList()
		{
			var model = new AdminSongsViewModel
			{
				Songs = _context.Songs  
				.Include(s => s.Genres)						//Include() metodunu yazarak songs'un genre bilgilerini aldık left join yaptık
				.Select(s => new AdminSongViewModel
				{
					SongId=s.SongId,
					Name = s.Name,
					Description= s.Description,
					ImageUrl= s.ImageUrl,
					Genres = s.Genres.ToList()
				}).ToList()
			};
			return View(model);
		}
	}
}
