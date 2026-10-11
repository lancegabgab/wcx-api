
using Microsoft.AspNetCore.Mvc;
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
		public async Task<IActionResult> GetAll()
		{
			var result = await _scheduleService.GetAllAsync();

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
	}
}