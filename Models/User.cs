using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace wcx_api.Models
{
	public class User : IdentityUser
	{
		[Required]
		[MaxLength(100)]
		public string FirstName { get; set; } = string.Empty;

		[MaxLength(100)]
		public string? MiddleName { get; set; }

		[Required]
		[MaxLength(100)]
		public string LastName { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}

}
