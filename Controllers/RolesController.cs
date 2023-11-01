using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp.Identity;

namespace MusicApp.Controllers
{
	public class RolesController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;
		public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public async Task<IActionResult> RoleList()
		{
			var model = await _roleManager.Roles.ToListAsync();
			return View(model);
		}

		//public IActionResult RoleCreate()
		//{
		//	return View();
		//}

		private IEnumerable<AppRole> GetRoles()
		{
			//var model = _roleManager.Roles.Select(r=>new AppRole
			//{
			//	Id=r.Id,
			//	Name=r.Name,
			//}).ToList();
			//return model;

			return _roleManager.Roles.ToList();
		}
		[HttpPost]
		public async Task<IActionResult> RoleCreate(AppRole model)
		{
			if (ModelState.IsValid)
			{
				var result = await _roleManager.CreateAsync(model);

				if (result.Succeeded)
				{
					return RedirectToAction("RoleList");
				}
				foreach (var err in result.Errors) //eger hata/hatalar varsa yazdıralım
				{
					ModelState.AddModelError("", err.Description);
				}
			}
			return View("RoleList", GetRoles());
		}
		[HttpPost]
		public async Task<IActionResult> RoleDelete(string name)
		{
			var role = await _roleManager.FindByNameAsync(name);
			if (role != null)
			{
				await _roleManager.DeleteAsync(role);
			}
			return RedirectToAction("RoleList");
		}
	}
}
