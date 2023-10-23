using MusicApp.Data;
using MusicApp.Entity;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace MusicApp.Validators
{
	public class ArtistNameAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
		{	
			var dbContext = (SongContext)validationContext.GetService(typeof(SongContext));

			if (value != null)
			{
				var artistName = value.ToString();
				var existingArtist = dbContext.Artists.FirstOrDefault(a => a.ArtistName == artistName);
				if (existingArtist != null)
				{
					return new ValidationResult($"{artistName} already exist");
				}
			}

			return ValidationResult.Success;
		}

	}
}
