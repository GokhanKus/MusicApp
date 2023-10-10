﻿using Microsoft.EntityFrameworkCore;
using MusicApp.Entity;

namespace MusicApp.Data
{
	public static class DataSeeding
	{
		public static void Seed(IApplicationBuilder app)
		{
			var scope = app.ApplicationServices.CreateScope();
			var context = scope.ServiceProvider.GetService<SongContext>();



			context.Database.Migrate();

			var genres = new List<Genre>
			{
				new Genre
				{
					//GenreId = 1, 
					Name = "Pop",
					Songs = new List<Song>
					{
						new Song
						{
							//SongId = 1,
							Name = "Song_1",
							Description = "Description_1",
							ImageUrl = "No_Image.jpg"

						}
					}
				},
				new Genre {Name="Rock"},
				new Genre {Name="Rap"},
				new Genre {Name="Metal"},
				new Genre {Name="Jazz"},
			};

			var songs = new List<Song>
			{
				new Song
				{
					Name="Song_2",
					Description ="Description_2",
					ImageUrl = "No_Image.jpg",
					Genres = new List<Genre>
					{
						genres[0], new Genre{Name = "newly added one"}, genres[1], genres[1]
					}
				},
				new Song
				{
					Name="Song_3",
					Description = "Description_3",
					ImageUrl = "No_Image.jpg",
					Genres = new List<Genre>
					{
						genres[1],genres[3],genres[4]
					}
				},
				new Song
				{
					Name="Song_4",
					Description = "Description_4",
					ImageUrl = "No_Image.jpg",
					Genres = new List<Genre>
					{
						genres[0]
					}
				},
				new Song
				{
					Name="Song_5",
					Description = "Description_5",
					ImageUrl = "No_Image.jpg",
					Genres = new List<Genre>
					{
						genres[3],genres[4]
					}
				},
				new Song
				{
					Name="Song_6",
					Description = "Description_6",
					ImageUrl = "No_Image.jpg",
					Genres = new List<Genre>
					{
						genres[2],genres[3]
					}
				},
				new Song
				{
					Name="Song_7",
					Description = "Description_7",
					ImageUrl = "No_Image.jpg",
					Genres = new List<Genre>
					{
						genres[3]
					}
				}
			};

			if (context.Database.GetPendingMigrations().Count() == 0) ////olusturulmus ancak uygulanmamıs olan migrationların listesini verir
			{
				//once genreler db'ye aktarılsın yoksa hata alırız, cünkü songs'ta genreId ataması yapıyoruz

				if (context.Songs.Count() == 0) context.Songs.AddRange(songs); //ilgili tabloya deger hic eklenmemisse burada ekleyelim
				if (context.Genres.Count() == 0) context.Genres.AddRange(genres);//ilgili tabloya deger hic eklenmemisse burada ekleyelim
				

				context.SaveChanges();
			}
		}
	}
}
