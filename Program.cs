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
//sifre, mail resetleme degistirme, mail onaylama gibi islemler icin gereken token bilgisini üretmek icin AddDefaultTokenProviders() yaz?yoruz.

//ilgili alanlarý degistirecegimiz alan (validation gibi)
//builder.Services.Configure<IdentityOptions>(options =>
//{

//});

//cookie ayarlarýný degistirecegimiz yer
//builder.Services.ConfigureApplicationCookie(options =>
//{
	
//});

// Add services to the container.
builder.Services.AddControllersWithViews();
	//.AddViewOptions(options => options.HtmlHelperOptions.ClientValidationEnabled = true); client tarafýnda validation, ama ... = false yaparsan server tarafýnda validation

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
}
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();//burayý da biz yazdýk yazmamýz lazým
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{Id?}");

app.Run();

