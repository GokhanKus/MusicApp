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
		private readonly SignInManager<AppUser> _signInManager;
		public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
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
				var result = await _userManager.CreateAsync(user, model.Password);
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

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user != null)
				{
					await _signInManager.SignOutAsync();
					var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
					if (result.Succeeded)
					{
						await _userManager.ResetAccessFailedCountAsync(user);
						await _userManager.SetLockoutEndDateAsync(user, null);
						return RedirectToAction("Index", "Home");
					}
					else if (result.IsLockedOut)
					{
						var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
						var timeLeft = lockoutDate.Value - DateTime.UtcNow;
						ModelState.AddModelError("", $"Your account has been locked. Please try {timeLeft.Minutes + 1}minutes later.");
					}
					else
					{
						ModelState.AddModelError("", $"Incorrect Password ");
					}
				}
				else
				{
					ModelState.AddModelError("", "Email could not find");
				}
			}
			return View(model);

		}

	}
}
