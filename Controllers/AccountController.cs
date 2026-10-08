using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using wcx_api.Services;

namespace wcx_api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService _accountService;

		public AccountController(
			IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet("profile")]
		public async Task<IActionResult> GetProfile()
		{
			try
			{
				var userId = User.FindFirstValue(
					ClaimTypes.NameIdentifier
				);

				if (string.IsNullOrEmpty(userId))
				{
					return Unauthorized(new
					{
						Success = false,
						Message = "User ID not found in token."
					});
				}

				var result = await _accountService.GetProfileAsync(
					userId
				);

				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new
				{
					Success = false,
					Message = ex.Message
				});
			}
		}
	}
}