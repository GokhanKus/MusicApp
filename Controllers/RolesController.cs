using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Identity;

namespace MusicApp.Controllers
{
	public class RolesController : Controller
	{
		private readonly RoleManager<AppRole> _roleManager;
        public RolesController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult RoleList()
		{
			var model = _roleManager.Roles;
			return View(model);
		}

		public IActionResult RoleCreate()
		{
			return View();
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
			return View(model);
		}
	}
}
