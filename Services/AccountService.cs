using Microsoft.AspNetCore.Identity;
using wcx_api.DTOs.Outputs;
using wcx_api.Models;

namespace wcx_api.Services
{
	public class AccountService
	{
		private readonly UserManager<User> _userManager;

		public AccountService(
			UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public async Task<Response<UserOutput>> GetProfileAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				throw new Exception("User not found.");
			}

			var roles = await _userManager.GetRolesAsync(user);

			var profile = new UserOutput
			{
				Id = user.Id,
				FirstName = user.FirstName,
				MiddleName = user.MiddleName,
				LastName = user.LastName,
				Email = user.Email!,
				Role = roles.FirstOrDefault() ?? string.Empty,
				CreatedAt = user.CreatedAt
			};

			return new Response<UserOutput>
			{
				Success = true,
				Message = "Profile retrieved successfully.",
				Data = profile
			};
		}
	}
}