using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using wcx_api.DTOs.Inputs;
using wcx_api.DTOs.Outputs;
using wcx_api.Models;

namespace wcx_api.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;

		public AuthService(UserManager<User> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
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

			await _userManager.AddToRoleAsync(user, "Agent");

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
				Message = "Registration successful.",
				Data = profile
			};
		}
		public async Task<Response<LoginOutput>> LoginAsync(LoginInput input)
		{
			// Find user by email
			var user = await _userManager.FindByEmailAsync(
				input.Email
			);

			if (user == null)
			{
				throw new Exception(
					"Invalid email or password."
				);
			}

			// Check password
			var passwordValid = await _userManager.CheckPasswordAsync(
				user,
				input.Password
			);

			if (!passwordValid)
			{
				throw new Exception(
					"Invalid email or password."
				);
			}

			// Get user's roles
			var roles = await _userManager.GetRolesAsync(user);

			var role = roles.FirstOrDefault();

			if (string.IsNullOrEmpty(role))
			{
				throw new Exception(
					"User does not have a role."
				);
			}

			// Create JWT claims
			var claims = new List<Claim>
			{
				new Claim(
					ClaimTypes.NameIdentifier,
					user.Id
				),

				new Claim(
					ClaimTypes.Email,
					user.Email!
				),

				new Claim(
					ClaimTypes.Name,
					user.UserName!
				),

				new Claim(
					ClaimTypes.Role,
					role
				)
			};

			// Get JWT settings
			var key = _configuration["Jwt:Key"];

			var issuer = _configuration["Jwt:Issuer"];

			var audience = _configuration["Jwt:Audience"];

			var expiresInMinutes = int.Parse(
				_configuration["Jwt:ExpiresInMinutes"]!
			);

			// Create signing key
			var securityKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(key!)
			);

			var credentials = new SigningCredentials(
				securityKey,
				SecurityAlgorithms.HmacSha256
			);

			// Create JWT
			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(
					expiresInMinutes
				),
				signingCredentials: credentials
			);

			// Convert JWT to string
			var tokenString = new JwtSecurityTokenHandler()
				.WriteToken(token);

			return new Response<LoginOutput>
			{
				Success = true,
				Message = "Login successful.",
				Data = new LoginOutput
				{
					Token = tokenString,
					Id = user.Id,
					FirstName = user.FirstName,
					MiddleName = user.MiddleName,
					LastName = user.LastName,
					Email = user.Email!,
					Role = role
				}
			};
		}

	}
}