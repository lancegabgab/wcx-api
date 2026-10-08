using System.ComponentModel.DataAnnotations;

namespace wcx_api.DTOs.Inputs
{
	public class UserInput
	{
		[Required]
		[MaxLength(100)]
		public string FirstName { get; set; } = string.Empty;

		[MaxLength(100)]
		public string? MiddleName { get; set; }

		[Required]
		[MaxLength(100)]
		public string LastName { get; set; } = string.Empty;

		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		[MinLength(8)]
		public string Password { get; set; } = string.Empty;

		[Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
