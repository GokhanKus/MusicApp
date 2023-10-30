using System.ComponentModel.DataAnnotations;

namespace MusicApp.IdentityModels
{
	public class LoginViewModel
	{
		[EmailAddress]
		public string Email { get; set; } = null!; //null olmayacagini söyleyelim yoksa uyarı veriyor

		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;
		public bool RememberMe { get; set; } = true;
	}
}
