using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp.Data;
using MusicApp.Identity;
using MusicApp.IdentityModels;

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
		private readonly RoleManager<AppRole> _roleManager;
		
        public UsersController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
			_roleManager = roleManager;
        }

        public IActionResult UserList()
		{
			var model = _userManager.Users;
			return View(model);
		}

		public async Task<IActionResult> UserEdit(string id)
		{
			if (id == null)	return RedirectToAction("UserList");
			
			var user = await _userManager.FindByIdAsync(id);

			if (user != null)
			{
				ViewBag.Roles = await _roleManager.Roles.Select(i => i.Name).ToListAsync();
				var model = new EditUserViewModel
				{
					Id = user.Id,
					FullName = user.FullName,
					Email = user.Email,
					SelectedRoles = await _userManager.GetRolesAsync(user)
				};
				return View(model);
			}
			return RedirectToAction("UserList");
		}
	}
}
