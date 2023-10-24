using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;

namespace MusicApp.Controllers
{
	public class BaseController : Controller
	{
		protected readonly SongContext _context;
		public BaseController(SongContext context)
		{
			_context = context;
		}
	}

}
