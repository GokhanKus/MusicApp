using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
using MusicApp.Models;
using System.Diagnostics;

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
        public IActionResult Index()
		{
			var model = new HomePageViewModel
			{
				PopularSongs = _context.Songs.ToList()
			};
			return View(model);
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