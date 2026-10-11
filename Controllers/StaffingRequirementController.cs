using Microsoft.AspNetCore.Mvc;
using wcx_api.DTOs.Inputs;
using wcx_api.DTOs.Outputs;
using wcx_api.Services;

namespace wcx_api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StaffingRequirementController : ControllerBase
	{
		private readonly StaffingRequirementService _service;

		public StaffingRequirementController(
			StaffingRequirementService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<ActionResult<Response<List<StaffingRequirementOutput>>>> GetAll()
		{
			var result = await _service.GetAllAsync();

			return Ok(result);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Response<StaffingRequirementOutput>>> GetById(int id)
		{
			var result = await _service.GetByIdAsync(id);

			if (!result.Success)
				return NotFound(result);

			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<Response<StaffingRequirementOutput>>> Create(
			[FromBody] StaffingRequirementInput input)
		{
			var result = await _service.CreateAsync(input);

			if (!result.Success)
				return BadRequest(result);

			return CreatedAtAction(
				nameof(GetById),
				new { id = result.Data!.Id },
				result);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<Response<StaffingRequirementOutput>>> Update(
			int id,
			[FromBody] StaffingRequirementInput input)
		{
			var result = await _service.UpdateAsync(id, input);

			if (!result.Success)
			{
				if (result.Message == "Staffing requirement not found.")
					return NotFound(result);

				return BadRequest(result);
			}

			return Ok(result);
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult<Response<bool>>> Delete(int id)
		{
			var result = await _service.DeleteAsync(id);

			if (!result.Success)
				return NotFound(result);

			return Ok(result);
		}
	}
}