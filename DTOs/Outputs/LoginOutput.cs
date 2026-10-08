namespace wcx_api.DTOs.Outputs
{
	public class LoginOutput
	{
		public string Token { get; set; } = string.Empty;
		public string Id { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string MiddleName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
	}
}
