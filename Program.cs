using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using MusicApp.Data;
using Microsoft.AspNetCore.Identity;
using MusicApp.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//sql conn string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SongContext>(options =>
options.UseSqlServer(connectionString));


builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<SongContext>().AddDefaultTokenProviders();
//sifre, mail resetleme degistirme, mail onaylama gibi islemler icin gereken token bilgisini üretmek icin AddDefaultTokenProviders() yazýyoruz.

//ilgili alanları degistirecegimiz alan (validation gibi)
builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireNonAlphanumeric = false; //özel karakter zorunlulugu olmasın
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;      //kucuk/buyuk harf zorunlulugu olmmasın
	options.Password.RequiredLength = 6;            //min 6 karakter

	options.User.RequireUniqueEmail = true; //aynı mail ile birden fazla kayit olusturulmasin
	options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
});

//cookie ayarlarını degistirecegimiz yer
//builder.Services.ConfigureApplicationCookie(options =>
//{

//});

// Add services to the container.
builder.Services.AddControllersWithViews();
//.AddViewOptions(options => options.HtmlHelperOptions.ClientValidationEnabled = true); client tarafında validation, ama ... = false yaparsan server tarafında validation

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
else //app.Environment.IsDevelopment()
{
	app.UseDeveloperExceptionPage();
	DataSeeding.Seed(app);
	IdentityDataSeeding.Seed(app);
}
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();//burayı da biz yazdık yazmamız lazım
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{Id?}");

app.Run();

