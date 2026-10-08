using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using wcx_api.Models;

namespace wcx_api.Data
{
	public class WcxDbContext : IdentityDbContext<User, Role, string>
	{
		public WcxDbContext(
			DbContextOptions<WcxDbContext> options)
			: base(options)
		{
		}
		public DbSet<StaffingRequirements> StaffingRequirements { get; set; }
		public DbSet<Schedule> Schedules { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Schedule>()
				.HasOne(s => s.Agent)
				.WithMany()
				.HasForeignKey(s => s.AgentId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
