using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wcx_api.Services;

namespace wcx_api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly UserService _userService;

		public UserController(UserService userService)
		{
			_userService = userService;
		}

		[HttpGet("agents")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllAgents()
		{
			var agents = await _userService.GetAllAgentsAsync();

			return Ok(agents);
		}
	}
}