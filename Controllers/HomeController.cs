using Microsoft.AspNetCore.Mvc;
//using MusicApp.Models;
using System.Diagnostics;

namespace MusicApp.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}