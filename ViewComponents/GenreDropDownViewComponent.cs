using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp.Data;

namespace MusicApp.ViewComponents
{
    public class GenreDropDownViewComponent : ViewComponent
    {
        private readonly SongContext _context;

        public GenreDropDownViewComponent(SongContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var genreNames =await _context.Genres.Select(g => g.GenreName).ToListAsync();
            var genreNames =await _context.Genres.ToListAsync();
            
            return View(genreNames);
        }
    }
}
