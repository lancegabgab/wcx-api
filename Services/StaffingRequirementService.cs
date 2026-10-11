using Microsoft.EntityFrameworkCore;
using wcx_api.Data;
using wcx_api.DTOs.Inputs;
using wcx_api.DTOs.Outputs;
using wcx_api.Models;

namespace wcx_api.Services
{
	public class StaffingRequirementService 
	{
		private readonly WcxDbContext _context;

		public StaffingRequirementService(WcxDbContext context)
		{
			_context = context;
		}

		public async Task<Response<List<StaffingRequirementOutput>>> GetAllAsync()
		{
			var requirements = await _context.StaffingRequirements
				.AsNoTracking()
				.Select(x => new StaffingRequirementOutput
				{
					Id = x.Id,
					Date = x.Date,
					StartTime = x.StartTime,
					EndTime = x.EndTime,
					RequiredAgents = x.RequiredAgents
				})
				.ToListAsync();

			return new Response<List<StaffingRequirementOutput>>
			{
				Success = true,
				Message = "Staffing requirements retrieved successfully.",
				Data = requirements
			};
		}

		public async Task<Response<StaffingRequirementOutput>> GetByIdAsync(int id)
		{
			var requirement = await _context.StaffingRequirements
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);

			if (requirement == null)
			{
				return new Response<StaffingRequirementOutput>
				{
					Success = false,
					Message = "Staffing requirement not found.",
					Data = null
				};
			}

			return new Response<StaffingRequirementOutput>
			{
				Success = true,
				Message = "Staffing requirement retrieved successfully.",
				Data = MapToOutput(requirement)
			};
		}

		public async Task<Response<StaffingRequirementOutput>> CreateAsync(
			StaffingRequirementInput input)
		{
			if (input.EndTime <= input.StartTime)
			{
				return new Response<StaffingRequirementOutput>
				{
					Success = false,
					Message = "End time must be later than start time.",
					Data = null
				};
			}

			if (input.RequiredAgents <= 0)
			{
				return new Response<StaffingRequirementOutput>
				{
					Success = false,
					Message = "Required agents must be greater than zero.",
					Data = null
				};
			}

			var requirement = new StaffingRequirements
			{
				Date = input.Date,
				StartTime = input.StartTime,
				EndTime = input.EndTime,
				RequiredAgents = input.RequiredAgents
			};

			_context.StaffingRequirements.Add(requirement);
			await _context.SaveChangesAsync();

			return new Response<StaffingRequirementOutput>
			{
				Success = true,
				Message = "Staffing requirement created successfully.",
				Data = MapToOutput(requirement)
			};
		}

		public async Task<Response<StaffingRequirementOutput>> UpdateAsync(
			int id,
			StaffingRequirementInput input)
		{
			var requirement = await _context.StaffingRequirements
				.FirstOrDefaultAsync(x => x.Id == id);

			if (requirement == null)
			{
				return new Response<StaffingRequirementOutput>
				{
					Success = false,
					Message = "Staffing requirement not found.",
					Data = null
				};
			}

			if (input.EndTime <= input.StartTime)
			{
				return new Response<StaffingRequirementOutput>
				{
					Success = false,
					Message = "End time must be later than start time.",
					Data = null
				};
			}

			if (input.RequiredAgents <= 0)
			{
				return new Response<StaffingRequirementOutput>
				{
					Success = false,
					Message = "Required agents must be greater than zero.",
					Data = null
				};
			}

			requirement.Date = input.Date;
			requirement.StartTime = input.StartTime;
			requirement.EndTime = input.EndTime;
			requirement.RequiredAgents = input.RequiredAgents;

			await _context.SaveChangesAsync();

			return new Response<StaffingRequirementOutput>
			{
				Success = true,
				Message = "Staffing requirement updated successfully.",
				Data = MapToOutput(requirement)
			};
		}

		public async Task<Response<bool>> DeleteAsync(int id)
		{
			var requirement = await _context.StaffingRequirements
				.FirstOrDefaultAsync(x => x.Id == id);

			if (requirement == null)
			{
				return new Response<bool>
				{
					Success = false,
					Message = "Staffing requirement not found.",
					Data = false
				};
			}

			_context.StaffingRequirements.Remove(requirement);
			await _context.SaveChangesAsync();

			return new Response<bool>
			{
				Success = true,
				Message = "Staffing requirement deleted successfully.",
				Data = true
			};
		}

		private static StaffingRequirementOutput MapToOutput(
			StaffingRequirements requirement)
		{
			return new StaffingRequirementOutput
			{
				Id = requirement.Id,
				Date = requirement.Date,
				StartTime = requirement.StartTime,
				EndTime = requirement.EndTime,
				RequiredAgents = requirement.RequiredAgents
			};
		}
	}
}