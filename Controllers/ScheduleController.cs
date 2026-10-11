
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using wcx_api.DTOs.Inputs;
using wcx_api.Services;

namespace wcx_api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ScheduleController : ControllerBase
	{
		private readonly ScheduleService _scheduleService;

		public ScheduleController(ScheduleService scheduleService)
		{
			_scheduleService = scheduleService;
		}

		// GET: api/Schedule
		[HttpGet]
		public async Task<IActionResult> GetAll(
		[FromQuery] string? agentId,
		[FromQuery] DateOnly? dateFrom,
		[FromQuery] DateOnly? dateTo,
		[FromQuery] TimeSpan? timeFrom,
		[FromQuery] TimeSpan? timeTo)
		{
			var result = await _scheduleService.GetAllAsync(agentId, dateFrom, dateTo, timeFrom, timeTo);
			return Ok(result);
		}

		// GET: api/Schedule/1
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			var result = await _scheduleService.GetByIdAsync(id);

			if (!result.Success)
			{
				return NotFound(result);
			}

			return Ok(result);
		}

		// POST: api/Schedule
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] ScheduleInput input)
		{
			var result = await _scheduleService.CreateAsync(input);

			if (!result.Success)
			{
				return BadRequest(result);
			}

			return CreatedAtAction(
				nameof(GetById),
				new { id = result.Data!.Id },
				result
			);
		}

		// PUT: api/Schedule/1
		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(
			int id,
			[FromBody] ScheduleInput input)
		{
			var result = await _scheduleService.UpdateAsync(id, input);

			if (!result.Success)
			{
				if (result.Message == "Schedule not found.")
				{
					return NotFound(result);
				}

				return BadRequest(result);
			}

			return Ok(result);
		}

		// DELETE: api/Schedule/1
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _scheduleService.DeleteAsync(id);

			if (!result.Success)
			{
				return NotFound(result);
			}

			return Ok(result);
		}

		[HttpGet("my-schedules")]
		[Authorize]
		public async Task<IActionResult> GetMySchedules(
		[FromQuery] DateOnly? dateFrom,
		[FromQuery] DateOnly? dateTo,
		[FromQuery] TimeSpan? timeFrom,
		[FromQuery] TimeSpan? timeTo
		)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}

			var result = await _scheduleService.GetMySchedulesAsync(
				userId,
				dateFrom,
				dateTo,
				timeFrom,
				timeTo
			);

			return Ok(result);
		}
	}
}