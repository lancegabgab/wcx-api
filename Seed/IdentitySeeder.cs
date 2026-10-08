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

		}
	}
}
