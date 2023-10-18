using Microsoft.EntityFrameworkCore;
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
					GenreName = "Pop",
					Songs = new List<Song>
					{
						new Song
						{
							//SongId = 1,
							SongName = "Song_1",
							Description = "Description_1",
							ImageUrl = "No_Image.jpg"
						}
					}
				},
				new Genre {GenreName="Rock"},
				new Genre {GenreName="Rap"},
				new Genre {GenreName="Metal"},
				new Genre {GenreName="Jazz"},
			};

			var songs = new List<Song>
			{
				new Song
				{
					SongName="Song_2",
					Description ="Description_2",
					ImageUrl = "No_Image.jpg",
					Genres = new List<Genre>
					{
						genres[0], new Genre{GenreName = "newly added one"}, genres[1], genres[1]
					}
				},
				new Song
				{
					SongName="Song_3",
					Description = "Description_3",
					ImageUrl = "No_Image.jpg",
					Genres = new List<Genre>
					{
						genres[1],genres[3],genres[4]
					}
				},
				new Song
				{
					SongName="Song_4",
					Description = "Description_4",
					ImageUrl = "No_Image.jpg",
					Genres = new List<Genre>
					{
						genres[0]
					}
				},
				new Song
				{
					SongName="Song_5",
					Description = "Description_5",
					ImageUrl = "No_Image.jpg",
					Genres = new List<Genre>
					{
						genres[3],genres[4]
					}
				},
				new Song
				{
					SongName="Song_6",
					Description = "Description_6",
					ImageUrl = "No_Image.jpg",
					Genres = new List<Genre>
					{
						genres[2],genres[3]
					}
				},
				new Song
				{
					SongName="Song_7",
					Description = "Description_7",
					ImageUrl = "No_Image.jpg",
					Genres = new List<Genre>
					{
						genres[3]
					}
				}
			};

			var artists = new List<Artist>
			{
				new Artist
				{
					ArtistName = "Artist_1",
					Nationality = "The USA",
					Songs = new List<Song>
					{
						songs[0],songs[2],	
					}
				},
				new Artist
				{
					ArtistName = "Artist_2",
					Nationality = "Germany",
					Songs = new List<Song>
					{
						songs[1],songs[3],	
					}
				}
			};

			var album = new List<Album>
			{
				new Album
				{
					AlbumName = "Album_1",
					Songs =new List<Song>
					{
						songs[2],songs[4],
					}
				},
				new Album
				{
					AlbumName = "Album_2",
					Songs =new List<Song>
					{
						songs[0],songs[4],
					}
				}
			};



			if (context.Database.GetPendingMigrations().Count() == 0) ////olusturulmus ancak uygulanmamıs olan migrationların listesini verir
			{
				//once genreler db'ye aktarılsın yoksa hata alırız, cünkü songs'ta genreId ataması yapıyoruz

				if (context.Songs.Count() == 0) context.Songs.AddRange(songs); //ilgili tabloya deger hic eklenmemisse burada ekleyelim
				if (context.Genres.Count() == 0) context.Genres.AddRange(genres);//ilgili tabloya deger hic eklenmemisse burada ekleyelim
				if (context.Artists.Count() == 0) context.Artists.AddRange(artists);
				if (context.Albums.Count() == 0) context.Albums.AddRange(album);
				

				context.SaveChanges();
			}
		}
	}
}
