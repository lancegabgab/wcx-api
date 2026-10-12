using Microsoft.AspNetCore.Identity;
using wcx_api.Models;

namespace wcx_api.Seed
{
	public static class IdentitySeeder
	{
		public static async Task SeedAsync(
			RoleManager<Role> roleManager, UserManager<User> userManager)
		{
			if (!await roleManager.RoleExistsAsync("Admin"))
			{
				await roleManager.CreateAsync(new Role
				{
					Name = "Admin"
				});
			}

			if (!await roleManager.RoleExistsAsync("Agent"))
			{
				await roleManager.CreateAsync(new Role
				{
					Name = "Agent"
				});
			}

			var admin = await userManager.FindByEmailAsync(
				"lancegabgab@gmail.com"
			);

			if (admin != null)
			{
				// Remove Agent role
				await userManager.RemoveFromRoleAsync(admin, "Agent");

				// Add Admin role
				if (!await userManager.IsInRoleAsync(admin, "Admin"))
				{
					await userManager.AddToRoleAsync(admin, "Admin");
				}
			}

			var agents = new[]
{
	new User
	{
		FirstName = "Lorna",
		MiddleName = "Lorenzo",
		LastName = "Cruz",
		UserName = "lorna@gmail.com",
		Email = "lorna@gmail.com",
		CreatedAt = DateTime.UtcNow
	},
	new User
	{
		FirstName = "Jessie",
		MiddleName = "Autida",
		LastName = "Cruz",
		UserName = "jessie@gmail.com",
		Email = "jessie@gmail.com",
		CreatedAt = DateTime.UtcNow
	},
	new User
	{
		FirstName = "Charles",
		MiddleName = "Lander",
		LastName = "Cruz",
		UserName = "charles@gmail.com",
		Email = "charles@gmail.com",
		CreatedAt = DateTime.UtcNow
	}
};

			foreach (var agent in agents)
			{
				var existingAgent = await userManager.FindByEmailAsync(
					agent.Email!
				);

				if (existingAgent == null)
				{
					var result = await userManager.CreateAsync(
						agent,
						"Agent@12345"
					);

					if (!result.Succeeded)
					{
						throw new Exception(
							$"Failed to create {agent.Email}: " +
							string.Join(", ",
								result.Errors.Select(e => e.Description))
						);
					}

					existingAgent = agent;
				}

				if (!await userManager.IsInRoleAsync(existingAgent, "Agent"))
				{
					var roleResult = await userManager.AddToRoleAsync(
						existingAgent,
						"Agent"
					);

					if (!roleResult.Succeeded)
					{
						throw new Exception(
							$"Failed to assign Agent role to {existingAgent.Email}: " +
							string.Join(", ",
								roleResult.Errors.Select(e => e.Description))
						);
					}
				}
			}

		}
	}
}
