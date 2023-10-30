using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicApp.Identity;

namespace MusicApp.Data
{
	public static class IdentityDataSeeding
	{
		private const string adminUser = "admin";
		private const string adminPassword = "Admin_123";

		public static async void Seed(IApplicationBuilder app)
		{
			var scope = app.ApplicationServices.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<SongContext>();

			if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.Migrate(); //olusturulmus ancak uygulanmamıs migration varsa update-database islemi yapalım
			}
			var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>(); //buradaki IdentityUser'dan kalıttığımız AppUser, bizim AspNetUsers tablomuza karsılık geliyor
																																//userManager uzerinden artık bir kullanıcı olusturabiliriz.

			var user = await userManager.FindByNameAsync(adminUser);
			if (user == null)
			{
				user = new AppUser
				{
					FullName = "Gokhan Kus",
					UserName = adminUser,
					Email = "gkus1998@gmail.com",
					PhoneNumber = "1234567890"
				};
				await userManager.CreateAsync(user, adminPassword);
			}
		}
	}
}
