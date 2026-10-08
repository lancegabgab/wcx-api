using Microsoft.AspNetCore.Identity;
using wcx_api.DTOs.Outputs;
using wcx_api.Models;

namespace wcx_api.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;

		public UserService(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public async Task<List<UserOutput>> GetAllAgentsAsync()
		{
			var users = await _userManager.GetUsersInRoleAsync("Agent");

			var result = new List<UserOutput>();

			foreach (var user in users)
			{
				result.Add(new UserOutput
				{
					Id = user.Id,
					FirstName = user.FirstName,
					MiddleName = user.MiddleName,
					LastName = user.LastName,
					Email = user.Email ?? string.Empty,
					Role = "Agent",
					CreatedAt = user.CreatedAt
				});
			}

			return result;
		}
	}
}