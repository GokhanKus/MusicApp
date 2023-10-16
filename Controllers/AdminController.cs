using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp.Data;
using MusicApp.Entity;
using MusicApp.Models;
using System.IO;
using System.Threading;
using static System.Net.WebRequestMethods;


namespace MusicApp.Controllers
{
	public class AdminController : Controller
	{
		private readonly SongContext _context;
		public AdminController(SongContext context)
		{
			_context = context; //inject işlemi
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult AddSong()
		{
			ViewBag.Genres = _context.Genres.ToList();
			return View(new AdminCreateSongModel());
		}

		[HttpPost]
		public IActionResult AddSong(AdminCreateSongModel model) //model bilgilerini sayfaya post edeceğiz
		{
			//if (model.Name == null)
			//{
			//	ModelState.AddModelError("", "Please enter song's name");
			//}

			//if (ModelState.IsValid)
			//{

			//}
			var entity = new Song
			{
				Name = model.Name,
				Description = model.Description,
				ReleaseDate = model.ReleaseDate,
				ImageUrl = "No_Image.jpg"
			};

			foreach (var id in model.GenreIds)
			{
				entity.Genres.Add(_context.Genres.FirstOrDefault(i => i.GenreId == id));
			}

			_context.Songs.Add(entity);
			_context.SaveChanges();

			return RedirectToAction("SongList");
		}
		public IActionResult SongList()
		{
			var model = new AdminSongsViewModel
			{
				Songs = _context.Songs
				.Include(s => s.Genres)                     //Include() metodunu yazarak songs'un genre bilgilerini aldık left join yaptık
				.Select(s => new AdminSongViewModel
				{
					SongId = s.SongId,
					Name = s.Name,
					ReleaseDate = s.ReleaseDate,
					Description = s.Description,
					ImageUrl = s.ImageUrl,
					Genres = s.Genres.ToList()
				}).ToList()
			};
			return View(model);
		}

		public IActionResult UpdateSong(int? id)
		{
			if (id == null) return NotFound();

			var entity = _context.Songs.Select(s => new AdminEditSongModel
			{
				SongId = s.SongId,
				Name = s.Name,
				Description = s.Description,
				ImageUrl = s.ImageUrl,
				ReleaseDate = s.ReleaseDate,
				GenreIds = s.Genres.Select(g => g.GenreId).ToArray()
			}).FirstOrDefault(s => s.SongId == id);

			ViewBag.Genres = _context.Genres.ToList();                       //bu bilgileri sayfaya taşıyalım

			if (entity == null) return NotFound();

			return View(entity);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateSong(AdminEditSongModel model, int[] genreIds, IFormFile file)
		{
			var entity = _context.Songs.Include("Genres").FirstOrDefault(s => s.SongId == model.SongId);
			if (entity == null) return NotFound();

			entity.Name = model.Name;
			entity.Description = model.Description;
			entity.ReleaseDate = model.ReleaseDate;

			if (file != null)                                                                                       //image bilgisi var mı?
			{
				var extension = Path.GetExtension(file.FileName);                                                   //resimin uzantı bilgisini aldık (jpg,jpeg,png,etc)
				var fileName = string.Format($"{Guid.NewGuid()}{extension}");                                       //burası bize eşsiz bir image name verir
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\", fileName);               //ana dizin yolu C:\ASP.NET_Projects\MusicApp\wwwroot\img\ornekresim.jpg olur.
				entity.ImageUrl = fileName;
				//using kullanarak nesne ile isimiz bitince bellekten silinsin																							//using ifadesi: using ifadesi, IDisposable arabirimine sahip nesnelerin kullanıldıktan sonra temizlenmesi için kullanılır. Bu ifade ile belirtilen nesneler kod bloğundan çıktıktan sonra otomatik olarak kapatılır ve kaynaklar serbest bırakılır.
				using (var stream = new FileStream(path, FileMode.Create))
				{
					//file.CopyTo(stream); //senkron versiyonu
					await file.CopyToAsync(stream);                                                                 //async oldugu icin dosyanın kaydedilmesini bekliyor olmalı (await) bu cs'nin en altında detaylı bilgi var
				}
			}

			entity.Genres = genreIds.Select(id => _context.Genres.FirstOrDefault(i => i.GenreId == id)).ToList();   //genreIdse gelen id'ler ile genres db'deki idlerden eşlesenleri liste olarak döndürür

			_context.SaveChanges();
			return RedirectToAction("SongList");
		}

		[HttpPost]
		public IActionResult RemoveSong(int songId)
		{
			var entity = _context.Songs.Find(songId);
			if (entity != null)
			{
				_context.Songs.Remove(entity);
				_context.SaveChanges();
			}
			return RedirectToAction("SongList");
		}
		
		public IActionResult GenreList()
		{
			return View(GetGenres());
		}
		private AdminGenresViewModel GetGenres()
		{
			var model = new AdminGenresViewModel
			{
				Genres = _context.Genres.Select(g => new AdminGenreViewModel
				{
					GenreId = g.GenreId,
					Name = g.Name,
					Count = g.Songs.Count
				}).ToList()
			};
			return model;
		}
		[HttpPost] //httppost yazmasak da çalışıyor?
		public IActionResult GenreCreate(AdminGenresViewModel model)
		{
			_context.Genres.Add(new Genre { Name = model.Name });
			_context.SaveChanges();
			return RedirectToAction("GenreList");
		}
		[HttpPost] //httppost yazmasak da çalışıyor?
		public IActionResult GenreDelete(int genreId)
		{
			var entity = _context.Genres.Find(genreId);
			if (entity != null)
			{
				_context.Genres.Remove(entity);
				_context.SaveChanges();
			}
			return RedirectToAction("GenreList");
		}
	}
}


/*
 * file.CopyTo(stream); (Senkron Kopyalama):
Bu kod, dosyayı senkron bir şekilde kopyalar. Senkron metotlar, işlemlerin sonuçlanmasını bekler ve işlem tamamlanana kadar diğer işlemleri bloke eder. Yani, file.CopyTo(stream); satırı çalıştırıldığında, dosyanın kopyalanması tamamlanana kadar işlem durur ve kontrol akışı bu satırda bekler.
Bu senkron metot, küçük dosyalarla çalışırken genellikle uygun olabilir. Ancak büyük dosyalar veya ağ işlemleri gibi yavaş işlemlerle uğraşırken, senkron işlemler uygulamanın performansını düşürebilir. Bu durumda, asenkron yöntemler tercih edilir.

await file.CopyToAsync(stream); (Asenkron Kopyalama):
Bu kod, dosyayı asenkron bir şekilde kopyalar. Asenkron metotlar, işlemleri bekletmeden devam edebilen iş parçacıkları (threads) kullanarak çalışırlar. Bu nedenle, dosyanın kopyalanması işlemi devam ederken diğer işlemler çalışabilir. Asenkron yöntemler, özellikle büyük veri transferleri veya ağ işlemleri gibi yoğun işlemlerle uğraşırken uygulamanın daha duyarlı olmasını sağlayabilir.

Yani, await file.CopyToAsync(stream); ifadesi, dosyanın kopyalanma işlemi tamamlanana kadar bekler, ancak beklerken diğer işlemlerin çalışmasına izin verir. Bu, uygulamanın daha etkili ve hızlı çalışmasını sağlar, çünkü asenkron işlemler bloklayıcı olmaz. Asenkron yapılar, özellikle kullanıcı arayüzüyle etkileşimli uygulamalar ve sunucu tabanlı uygulamalar gibi senkron işlem performansını etkileyebilecek durumlarda kullanışlıdır.
 */