using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using wcx_api.Models;

namespace wcx_api.Data
{

	public class WcxDbContext : IdentityDbContext<User>
	{
		public WcxDbContext(
			DbContextOptions<WcxDbContext> options)
			: base(options)
		{
		}

	}
}
