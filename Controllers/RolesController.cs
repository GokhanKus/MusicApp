using Microsoft.AspNetCore.Mvc;

namespace MusicApp.Controllers
{
	public class RolesController : Controller
	{
		public IActionResult RoleList()
		{
			return View();
		}
	}
}
