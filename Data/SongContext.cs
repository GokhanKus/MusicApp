using Microsoft.EntityFrameworkCore;
using MusicApp.Entity;

namespace MusicApp.Data
{
	public class SongContext:DbContext
	{
        public SongContext()
        {
            
        }
        public SongContext(DbContextOptions<SongContext> options) : base(options)
		{

		}
		public DbSet<Song> Songs { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<Artist> Artists { get; set; }
		public DbSet<Album> Albums { get; set; }

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) bunu burada tanımlamak yerine appsettings.json icerisinde tanımladık
		//{
		//	optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog=moviesDb; Integrated Security=SSPI;");
		//}
	}
}
