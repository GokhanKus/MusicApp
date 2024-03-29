﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MusicApp.Data;
using MusicApp.Entity;
using MusicApp.Models;
using System.IO;
using System.Threading;
using static System.Net.WebRequestMethods;


namespace MusicApp.Controllers
{
	[Authorize(Roles = "Admin")] //admin rolünde olmayan kullanicilar bu controller altındaki sayfaya giremesin
	public class AdminController : BaseController
	{
		//private readonly SongContext _context;
		//basecontrollerdan kalıtım aldığımız için contexti injection islemini burada yapmayalım

		public AdminController(SongContext context) :base(context)
		{
			//_context = context; //inject işlemi
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

			if (ModelState.IsValid)
			{
				var entity = new Song
				{
					SongName = model.SongName,
					Description = model.Description,
					ReleaseDate = model.ReleaseDate,
					ImageUrl = "No_Image.jpg",
				};


				var existingArtist = _context.Artists.FirstOrDefault(a => a.ArtistName == model.ArtistName);

				if (existingArtist == null)
				{
					existingArtist = new Artist
					{
						ArtistName = model.ArtistName,
					};
					_context.Artists.Add(existingArtist);
				}

				// Artist ve Song ilişkisini veritabanına bırakın, otomatik olarak ArtistSong tablosunu güncelleyecektir
				entity.Artists.Add(existingArtist);

				foreach (var id in model.GenreIds)
				{
					entity.Genres.Add(_context.Genres.FirstOrDefault(i => i.GenreId == id));
				}

				_context.Songs.Add(entity);
				_context.SaveChanges();

				return RedirectToAction("SongList");
			}
			ViewBag.Genres = _context.Genres.ToList();
			return View(model);
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
					SongName = s.SongName,
					ReleaseDate = s.ReleaseDate,
					ImageUrl = s.ImageUrl,
					Genres = s.Genres.ToList(),
					ArtistNames = s.Artists.Select(a => a.ArtistName).ToList()
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
				SongName = s.SongName,
				Description = s.Description,
				ImageUrl = s.ImageUrl,
				ReleaseDate = s.ReleaseDate,
				ArtistNames = s.Artists.Select(a => a.ArtistName).ToList(),

				GenreIds = s.Genres.Select(g => g.GenreId).ToArray(),
			}).FirstOrDefault(s => s.SongId == id);

			// ArtistNames dizisini virgülle ayırarak birleştiriyoruz
			if (entity != null)
			{
				entity.ArtistNamesString = string.Join(", ", entity.ArtistNames);
			}

			ViewBag.Genres = _context.Genres.ToList();                       //bu bilgileri sayfaya taşıyalım

			if (entity == null) return NotFound();

			return View(entity);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateSong(AdminEditSongModel model, int[] genreIds, IFormFile? file)
		{ //file=null demezsek ya da "?" koymazsak şarkıyı güncelleme isleminde bizden resim istiyor, ancak biz boyle bir validation kuralı belirtmemistik.(the file field is required) 
			if (ModelState.IsValid)
			{
				var entity = _context.Songs.Include(g => g.Genres).Include(a => a.Artists).FirstOrDefault(s => s.SongId == model.SongId);
				if (entity == null) return NotFound();

				entity.SongName = model.SongName;
				entity.Description = model.Description;
				entity.ReleaseDate = model.ReleaseDate;

				var artistNames = model.ArtistNamesString
				.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(artistName => artistName.Trim()) // Boşlukları temizle
				.ToList();
				entity.Artists.Clear(); // Şarkının ilişkili sanatçılarını temizle

				foreach (var artistName in artistNames)
				{
					var artist = _context.Artists.FirstOrDefault(a => a.ArtistName == artistName);
					if (artist != null)
					{
						entity.Artists.Add(artist); // Şarkıya sanatçıyı ekle,

					}
					else
					{
						// Sanatçı bulunamazsa, yeni sanatçı oluşturup ekleyebiliriz.
						var newArtist = new Artist { ArtistName = artistName };
						entity.Artists.Add(newArtist);
					}
				}

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

			ViewBag.Genres = _context.Genres.ToList();
			return View(model);
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
					GenreName = g.GenreName,
					Count = g.Songs.Count
				}).ToList()
			};
			return model;
		}


		[HttpPost] //httppost yazmasak da çalışıyor?
		public IActionResult GenreCreate(AdminGenresViewModel model)
		{
			if (ModelState.IsValid)
			{
				_context.Genres.Add(new Genre { GenreName = model.GenreName });
				_context.SaveChanges();
				return RedirectToAction("GenreList");
			}
			return View("GenreList", GetGenres());
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
		public IActionResult GenreUpdate(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var entity = _context.Genres  // Songs ilişkisini dahil et
		
				.Select(g => new AdminGenreEditViewModel
				{
					GenreId = g.GenreId,
					GenreName = g.GenreName,
					Songs = g.Songs.Select(i => new AdminSongViewModel
					{
						SongId = i.SongId,
						SongName = i.SongName,
						ImageUrl = i.ImageUrl,
						ReleaseDate = i.ReleaseDate,
						
						
					}).ToList()
				}).FirstOrDefault(g => g.GenreId == id);
			
			if (entity == null)
			{
				return NotFound();
			}

			return View(entity);
		}
		[HttpPost]
		public IActionResult GenreUpdate(AdminGenreEditViewModel model, int[] songIds)
		{
			if (ModelState.IsValid)
			{
				var entity = _context.Genres.Include(g => g.Songs).FirstOrDefault(i => i.GenreId == model.GenreId);

				if (entity == null) return NotFound();

				entity.GenreName = model.GenreName;
				foreach (var id in songIds)
				{
					entity.Songs.Remove(entity.Songs.FirstOrDefault(s => s.SongId == id));
				}
				_context.SaveChanges();
				return RedirectToAction("GenreList");
			}
			return View(model);
		}

		public IActionResult ArtistList()
		{
			return View(GetArtists());
		}

		private AdminArtistsViewModel GetArtists()
		{
			var model = new AdminArtistsViewModel
			{
				Artists = _context.Artists.Select(a => new AdminArtistViewModel
				{
					ArtistId = a.ArtistId,
					Count = a.Songs.Count,
					ArtistName = a.ArtistName,
				}).ToList()
			};
			return model;
		}

		[HttpPost]
		public IActionResult ArtistCreate(AdminArtistsViewModel model)
		{
			if (ModelState.IsValid)
			{
				_context.Artists.Add(new Artist
				{
					ArtistName = model.ArtistName

				});
				_context.SaveChanges();
				return RedirectToAction("ArtistList");
			}
			return View("ArtistList", GetArtists());
		}
		public IActionResult ArtistUpdate(int? id)
		{
			
			if (id == null)
			{
				return NotFound();
			}
			var entity = _context.Artists
				.Select(a => new AdminArtistEditModel
				{
					ArtistId = a.ArtistId,
					ArtistName = a.ArtistName,
					Nationality = a.Nationality,
					Songs = a.Songs.Select(i => new AdminSongViewModel
					{
						SongId = i.SongId,
						SongName = i.SongName,
						ImageUrl = i.ImageUrl,
						ReleaseDate = i.ReleaseDate,
					}).ToList()
				}).FirstOrDefault(a => a.ArtistId == id);

			ViewBag.ArtistNames = entity.ArtistName;


			if (entity == null)
			{
				return NotFound();
			}

			return View(entity);
		}

		[HttpPost]
		public IActionResult ArtistUpdate(AdminArtistEditModel model, int[] songIds)
		{
			if (ModelState.IsValid)
			{
				var entity = _context.Artists.Include(a => a.Songs).FirstOrDefault(i => i.ArtistId == model.ArtistId);

				if (entity == null) return NotFound();

				entity.ArtistName = model.ArtistName;
				entity.Nationality = model.Nationality;
				foreach (var id in songIds)
				{
					entity.Songs.Remove(entity.Songs.FirstOrDefault(s => s.SongId == id));
				}
				_context.SaveChanges();
				return RedirectToAction("ArtistList");
				
			}
			return View(model);
		}

		[HttpPost]
		public IActionResult ArtistDelete(int artistId)
		{
			var entity = _context.Artists.Find(artistId);
			if (entity != null)
			{
				_context.Remove(entity);
				_context.SaveChanges();
			}
			return RedirectToAction("ArtistList");
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