using Microsoft.AspNetCore.Mvc;
using wcx_api.DTOs.Inputs;
using wcx_api.Services;

namespace wcx_api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(UserInput input)
		{
			try
			{
				var response = await _authService.RegisterAsync(input);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return BadRequest(new
				{
					success = false,
					message = ex.Message
				});
			}
		}

	}
}
