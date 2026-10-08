using Microsoft.AspNetCore.Identity;

namespace wcx_api.Models
{
	public class User : IdentityUser
	{
		public string FullName { get; set; } = string.Empty;
	}

}
