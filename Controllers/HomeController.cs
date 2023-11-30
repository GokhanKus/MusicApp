using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
using MusicApp.Entity;
using MusicApp.Models;
using System.Diagnostics;
using X.PagedList;

namespace MusicApp.Controllers
{

	public class HomeController : BaseController
	{
		//private readonly SongContext _context;

		//basecontrollerdan kalıtım aldığımız için contexti injection islemini burada yapmayalım
		public HomeController(SongContext context) : base(context)
		{
			//_context = songContext; //injection islemi
		}
		public IActionResult Index(int page = 1)
		{
			//	var songs = _context.Songs.ToList();
			//	IPagedList<Song> pagedSongs = songs.ToPagedList(page, 10);
			var model = new HomePageViewModel
			{
				PopularSongs = _context.Songs.ToList()
			};
			IPagedList<Song> pagedList = model.PopularSongs.ToPagedList(page, 4);// 1.sayfadan baslasin ve 4 item gostersin

			return View(pagedList);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


		//private readonly ILogger<HomeController> _logger;
		//public HomeController(ILogger<HomeController> logger)
		//{
		//	_logger = logger;
		//}

	}
}