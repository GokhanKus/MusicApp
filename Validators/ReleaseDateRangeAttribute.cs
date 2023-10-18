using MusicApp.Models;
using System.ComponentModel.DataAnnotations;

namespace MusicApp.Validators
{
	public class ReleaseDateRangeAttribute : ValidationAttribute
	{
		public ReleaseDateRangeAttribute(int minYear)
		{
			MinYear = minYear;
		}
		public int MinYear { get; set; }
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value != null)
			{
				int enteredValue = (int)value;
				int currentYear = DateTime.Now.Year;
				if (enteredValue >= MinYear && enteredValue <= currentYear)
				{
					return ValidationResult.Success;

				}
				return new ValidationResult($"The ReleaseDate field must be between {MinYear} and {currentYear}.");
			}
			return ValidationResult.Success;
		}
	}


	//private int minYear = 1800;
	//private int maxYear = DateTime.Now.Year;
	//public override bool IsValid(object? value)
	//{
	//	if (value is int year)
	//	{
	//		return year >= minYear && year <= maxYear;
	//	}
	//	return false;
	//}
	//public override string FormatErrorMessage(string releaseDate)
	//{
	//	return $"The {releaseDate} field must be between {minYear} and {maxYear}.";
	//}


}
