using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MusicApp.Identity;
using MusicApp.IdentityModels;


namespace MusicApp.TagHelpers
{
	[HtmlTargetElement("td", Attributes = "asp-role-users")] //td etiketi altında calısan "asp-role-users" tagi
	public class RoleUsersTagHelper : TagHelper
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;

		public RoleUsersTagHelper(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}
		[HtmlAttributeName("asp-role-users")]
		public string RoleId { get; set; } = null!;

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			var userNames = new List<string>();
			var role = await _roleManager.FindByIdAsync(RoleId);

			if (role != null && role.Name != null)
			{	//foreachte in _userManager.Users seklindeydi ve "there is already an open datareader associated with this connection which must be closed first." hatası alıyordum
				var list = _userManager.Users.ToList(); //boyle yaparak problem çözüldü veya devjson con stringte MultipleActiveResultSets=true yazılabilir.
				foreach (var user in list) 
				{
					if (await _userManager.IsInRoleAsync(user, role.Name)) //almıs oldugumuz userin ilgili role içerisinde olup olmama durumu, rolü var mı?
					{
						userNames.Add(user.UserName ?? ""); //rolü olan userları listeye alalım   (?? "") bu kısım user.userName null ise bos string gonder ""
					}
				}
				//output.Content.SetContent(userNames.Count()==0 ? "Kullanıcı yok" : string.Join(", ", userNames)); //yan yana yazmasın html kodu olarak liste seklinde alt alta gelsin 
				output.Content.SetHtmlContent(userNames.Count() == 0 ? "No Users Found" : SetHtml(userNames));
			}
		}
		private string SetHtml(List<string> userNames)
		{
			var html = "<ul>";

			foreach (var user in userNames)
			{
				html += $"<li>{user}</li>";
			}
			html += "</ul>";
			return html;
		}
	}
}
