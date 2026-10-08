using Microsoft.AspNetCore.Identity;

namespace wcx_api.Models
{
	public class Role : IdentityRole
	{
		public string Description { get; set; } = string.Empty;
	}
}
