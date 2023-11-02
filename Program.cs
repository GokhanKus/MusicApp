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

	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.SignIn.RequireConfirmedEmail = true; //bu özelliği kapatınca onaylı olmayan hesaplarda result.IsLockedOut çalışıyor?
});

//cookie ayarlarını degistirecegimiz yer
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Account/Login"; //burasi zaten default olarak boyle gelir yazmasak da olur, ancak istersek degistirebiliriz. kullanıcı kimliği doğrulanmadığında yönlendirileceği adrestir.
	options.AccessDeniedPath = "/Account/AccessDenied";//kullanici kayit olmustur, ancak rolü yoktur ve user erisme yetkisi olmayan kýsma erismek isterse erisemez ve bu durumda kullaniciyi yonlendirmek istedigimiz yer burasidir.
	options.ExpireTimeSpan = TimeSpan.FromDays(30); //default olarak 14 gundur bu, bir cookie olusturuldugu zaman 14 gun gecerli bir cookie oluyor user giris yaptýktan 14 gun sonra user hiç logout olmazsa bu cookinin süresi biter. bu süreyi degisterbiliriz 30 gun yapalým.
													//user bu zaman aralýgýnda tekrar logout login islemini yaparsa sure resetlenir

	options.SlidingExpiration = true;				//belirtilen sürenin yarısına gelindiğinde sürenin yenilenip yenilenmeyeceğini belirler.
});

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

