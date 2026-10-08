using Microsoft.AspNetCore.Identity;
using wcx_api.Models;

namespace wcx_api.Seed
{
	public static class IdentitySeeder
	{
		public static async Task SeedAsync(
			RoleManager<Role> roleManager)
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
		}
	}
}
