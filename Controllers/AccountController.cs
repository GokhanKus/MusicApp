using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Identity;
using MusicApp.IdentityModels;
using MusicApp.Interfaces;

namespace MusicApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IEmailSender _emailSender;
		public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IEmailSender emailSender)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
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
				IdentityResult result = await _userManager.CreateAsync(user, model.Password);               //var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);   //ilgili user icin bize bir token bilgisi üretsin ve program.cste AddDefaultTokenProvider()
					var url = Url.Action("ConfirmEmail", "Account", new { user.Id, token });

					await _emailSender.SendEmailAsync(user.Email,
						"Confirm Your E-Mail",
						$"Confirm your account please click <a href='http://localhost:4570{url}'>here</a> "
						);

					TempData["message"] = "Please click on the confirmation email in your email account.";

					return RedirectToAction("Login");

				}
				foreach (IdentityError err in result.Errors) //ilgili hata mesajlarını yazdıralım eğer valid değilse
				{
					ModelState.AddModelError("", err.Description);
				}
			}
			return View(model);
		}

		public async Task<IActionResult> ConfirmEmail(string id, string token)
		{
			if (id == null || token == null)
			{
				TempData["message"] = "invalid token";
				return View();
			}
			var user = await _userManager.FindByIdAsync(id);

			if (user != null)
			{
				var result = await _userManager.ConfirmEmailAsync(user, token);
				if (result.Succeeded)
				{
					TempData["message"] = "Your account has been confirmed";
					return RedirectToAction("Login");
				}
			}
			else
				TempData["message"] = "no user found";

			return View();
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

					if (!await _userManager.IsEmailConfirmedAsync(user)) //e-mail onaylanmadıysa;
					{
						ModelState.AddModelError("", "Confirm your account");
						return View(model);
					}

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
						ModelState.AddModelError("", $"Your account has been locked. Please try {timeLeft.Minutes + 1} minutes later.");
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
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync(); //application altındaki cookienin silinmesi?
			return RedirectToAction("Index", "Home");
		}
	}
}

/*Authentication	temel olarak 3 farklı kimlik doğrulama yöntemi vardır;
 
Cookie Based Authentication			=> tarayıcılarda kullanılan doğrulama mesela kullanıcı tarayıcıya bir mail ve parola ile bir login işlemi gerçekleştirince
biz kullanıcının tarayıcısına bir cookie(çerez) bırakıyoruz ve kullanıcı bu siteyi sonra tekrar ziyaret ederse
hatırlanması için gereken bazı bilgilerin tarayıcı belleğine saklanmasıdır. Yani user browserında saklı kalan bir bilgi ve buna cookie diyoruz.
Ve cookie kullanarak bir Authentication işlemi gerçekleştirebiliyoruz.
diyelim ki Kullanıcı bir login islemi gerceklestirdi (mail sifre bilgisi girdi) uygulama o kullanıcının tarayıcısına o cookieyi yani bilgiyi saklar ve 
daha sonra kullanıcı her seferinde uygulamayı talep ettiginde o cookie de server tarafına uygulamaya gonderilir ve
uygulama bu cookie icindeki bilgiye bakarak tekrar bir login islemine gerek kalmadan user'ın istediği kaynağı ona sunma islemidir

Token Based Authentication - JWT	=> (Json Web Token olarak da adlandırılır)
biz bir token bilgisi uygulamada olusturuyoruz ve bu token bilgisini usera gonderiyoruz ve user bu token bilgisini
her seferinde bu tokenı talep ettigi kaynağa, uygulamaya, tekrar göndermesi gerekiyor. bu yontem mobil uygulamalarda kullanılıyor

External Provider Authentication	=> google ile giris yap, facebook ile giris yap olayı budur.
eger sen google ile facebook ile vs. Authentication islemi yapabiliyorsan benim uygulamamda sana guveniyor ve bu Authentication islemini benim app tarafından da kabul etme islemidir
 
 */
