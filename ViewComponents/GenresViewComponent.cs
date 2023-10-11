using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;

namespace MusicApp.ViewComponents
{
	public class GenresViewComponent:ViewComponent
	{
		private readonly SongContext _context; //veritabanı ile etkilesim icin kullanılır
		public GenresViewComponent(SongContext context)
		{
			_context = context;  //injection islemi  
		}
		public IViewComponentResult Invoke()
		{

			ViewBag.SelectedGenre = RouteData.Values["id"];   //secilen film türünün isaretlenmesi(background mavi olsun mesela)

			return View(_context.Genres.ToList());
		}
	}
}
