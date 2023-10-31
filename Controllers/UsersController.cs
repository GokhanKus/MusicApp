using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
using MusicApp.Identity;

namespace MusicApp.Controllers
{
	public class UsersController : Controller
	{
		//Bu Injection islemi de oluyor.
		//private readonly SongContext _context;
        //public UsersController(SongContext context)
        //{
        //	_context = context;
        //}

		private readonly UserManager<AppUser> _userManager;
		
        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult UserList()
		{
			var model = _userManager.Users;
			return View(model);
		}
	}
}
