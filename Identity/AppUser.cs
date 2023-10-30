using Microsoft.AspNetCore.Identity;

namespace MusicApp.Identity
{
	public class AppUser:IdentityUser
	{
        public string? FullName{ get; set; }
    }
}
