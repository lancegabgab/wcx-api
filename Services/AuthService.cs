using Microsoft.AspNetCore.Identity;
using wcx_api.DTOs.Inputs;
using wcx_api.DTOs.Outputs;
using wcx_api.Models;
using wcx_api.Services;

namespace wcx_api.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<User> _userManager;
		public AuthService(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public async Task<Response<UserOutput>> RegisterAsync(UserInput input)
		{
			if (input.Password != input.ConfirmPassword)
			{
				throw new Exception("Passwords do not match.");
			}

			var user = new User
			{
				FirstName = input.FirstName,
				MiddleName = input.MiddleName,
				LastName = input.LastName,
				Email = input.Email,
				UserName = input.Email,
				CreatedAt = DateTime.UtcNow
			};

			var result = await _userManager.CreateAsync(
				user,
				input.Password
			);

			if (!result.Succeeded)
			{
				var errors = string.Join(
					", ",
					result.Errors.Select(error => error.Description)
				);

				throw new Exception(errors);
			}

			var profile = new UserOutput
			{
				Id = user.Id,
				FirstName = user.FirstName,
				MiddleName = user.MiddleName,
				LastName = user.LastName,
				Email = user.Email!,
				CreatedAt = user.CreatedAt
			};

			return new Response<UserOutput>
			{
				Success = true,
				Message = "Registration successful.",
				Data = profile
			};
		}

	}
}
