using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicApp.Identity;

namespace MusicApp.Data
{
	//username: admin, password: Admin_123, Email: gkus1998@gmail.com
	public static class IdentityDataSeeding
	{
		private const string adminUser = "admin1";
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
					FullName = "GokhanKus",
					UserName = adminUser,
					Email = "gokhankus98@gmail.com",
					PhoneNumber = "123456789"
				};
				await userManager.CreateAsync(user, adminPassword);
			}
		}
	}
}
