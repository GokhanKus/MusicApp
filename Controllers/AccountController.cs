using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Identity;
using MusicApp.IdentityModels;

namespace MusicApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;

		public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new AppUser
				{
					UserName = model.UserName,
					Email = model.Email,
					FullName = model.FullName,
				};
				var result = await _userManager.CreateAsync(user,model.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("Login");
				}
				foreach (IdentityError err in result.Errors) //ilgili hata mesajlarını yazdıralım eğer valid değilse
				{
					ModelState.AddModelError("", err.Description);
				}
			}
			return View(model);
		}
		public IActionResult Login()
		{
			return View();
		}

	}
}
