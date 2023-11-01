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
			if (id == null) return RedirectToAction("UserList");

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

		[HttpPost]
		public async Task<IActionResult> UserEdit(string id, EditUserViewModel model)
		{
			if (id != model.Id) return RedirectToAction("UserList");

			ViewBag.Roles = await _roleManager.Roles.Select(i => i.Name).ToListAsync();

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(model.Id);

				if (user != null)
				{
					user.Email = model.Email;
					user.FullName = model.FullName;

					var result = await _userManager.UpdateAsync(user);
					if (result.Succeeded && !string.IsNullOrEmpty(model.Password))
					{
						await _userManager.RemovePasswordAsync(user);
						await _userManager.AddPasswordAsync(user, model.Password);
					}
					if (result.Succeeded)
					{
						await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user)); //once userdan rolleri kaldıralım

						if (model.SelectedRoles != null)
						{
							await _userManager.AddToRolesAsync(user, model.SelectedRoles); //sonra usera rolleri atayalım
						}
						return RedirectToAction("UserList");
					}

					foreach (IdentityError err in result.Errors) //useri güncellerken hata alırsak ilgili mesajları yazdıralım
					{
						ModelState.AddModelError("", err.Description);
					}
				}
			}
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> UserDelete(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user != null)
			{
				var result = await _userManager.DeleteAsync(user);
				if (result.Succeeded)
				{
					return RedirectToAction("UserList");
				}
				foreach (var err in result.Errors)
				{
					ModelState.AddModelError("", err.Description);
				}
			}
			return RedirectToAction("UserList");
		}
	}
}
