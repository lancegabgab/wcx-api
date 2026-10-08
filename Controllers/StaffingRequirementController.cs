using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wcx_api.DTOs.Inputs;
using wcx_api.Services;

namespace wcx_api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = "Admin")]
	public class StaffingRequirementController : ControllerBase
	{
		private readonly IStaffingRequirementService _service;

		public StaffingRequirementController(
			IStaffingRequirementService service)
		{
			_service = service;
		}

		// GET: api/StaffingRequirement
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _service.GetAllAsync();

			return Ok(result);
		}

		// GET: api/StaffingRequirement/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var result = await _service.GetByIdAsync(id);

			if (result == null)
			{
				return NotFound();
			}

			return Ok(result);
		}

		// POST: api/StaffingRequirement
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create(
			StaffingRequirementInput dto)
		{
			if (dto.StartTime >= dto.EndTime)
			{
				return BadRequest(
					"Start time must be before end time.");
			}

			if (dto.RequiredAgents < 0)
			{
				return BadRequest(
					"Required agents cannot be negative.");
			}

			var result = await _service.CreateAsync(dto);

			return CreatedAtAction(
				nameof(GetById),
				new { id = result.Id },
				result);
		}

		// PUT: api/StaffingRequirement/{id}
		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(
			int id,
			StaffingRequirementInput dto)
		{
			if (dto.StartTime >= dto.EndTime)
			{
				return BadRequest(
					"Start time must be before end time.");
			}

			if (dto.RequiredAgents < 0)
			{
				return BadRequest(
					"Required agents cannot be negative.");
			}

			var updated = await _service.UpdateAsync(id, dto);

			if (!updated)
			{
				return NotFound();
			}

			return NoContent();
		}

		// DELETE: api/StaffingRequirement/{id}
		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var deleted = await _service.DeleteAsync(id);

			if (!deleted)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}