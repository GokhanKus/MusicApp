using MusicApp.Data;
using System.ComponentModel.DataAnnotations;

namespace MusicApp.Validators
{
	public class GenreNameAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var dbContext = (SongContext)validationContext.GetService(typeof(SongContext));

			if (value != null)
			{
				var genreName = value.ToString();
				var existingGenre = dbContext.Genres.FirstOrDefault(g=>g.GenreName == genreName);

				if (existingGenre != null)
				{
					return new ValidationResult($"{genreName} already exist");
				}
			}
			return ValidationResult.Success;
		}
	}
}
