using Microsoft.EntityFrameworkCore;
using wcx_api.Data;
using wcx_api.DTOs.Inputs;
using wcx_api.DTOs.Outputs;
using wcx_api.Models;

namespace wcx_api.Services
{
	public class StaffingRequirementService
		: IStaffingRequirementService
	{
		private readonly WcxDbContext _context;

		public StaffingRequirementService(WcxDbContext context)
		{
			_context = context;
		}

		public async Task<List<StaffingRequirementOutput>> GetAllAsync()
		{
			return await _context.StaffingRequirements
				.OrderBy(x => x.Date)
				.ThenBy(x => x.StartTime)
				.Select(x => new StaffingRequirementOutput
				{
					Id = x.Id,
					Date = x.Date,
					StartTime = x.StartTime,
					EndTime = x.EndTime,
					RequiredAgents = x.RequiredAgents
				})
				.ToListAsync();
		}

		public async Task<StaffingRequirementOutput?> GetByIdAsync(int id)
		{
			return await _context.StaffingRequirements
				.Where(x => x.Id == id)
				.Select(x => new StaffingRequirementOutput
				{
					Id = x.Id,
					Date = x.Date,
					StartTime = x.StartTime,
					EndTime = x.EndTime,
					RequiredAgents = x.RequiredAgents
				})
				.FirstOrDefaultAsync();
		}

		public async Task<StaffingRequirementOutput> CreateAsync(
			StaffingRequirementInput dto)
		{
			var requirement = new StaffingRequirements
			{
				Date = dto.Date.Date,
				StartTime = dto.StartTime,
				EndTime = dto.EndTime,
				RequiredAgents = dto.RequiredAgents
			};

			_context.StaffingRequirements.Add(requirement);

			await _context.SaveChangesAsync();

			return new StaffingRequirementOutput
			{
				Id = requirement.Id,
				Date = requirement.Date,
				StartTime = requirement.StartTime,
				EndTime = requirement.EndTime,
				RequiredAgents = requirement.RequiredAgents
			};
		}

		public async Task<bool> UpdateAsync(
			int id,
			StaffingRequirementInput dto)
		{
			var requirement =
				await _context.StaffingRequirements.FindAsync(id);

			if (requirement == null)
			{
				return false;
			}

			requirement.Date = dto.Date.Date;
			requirement.StartTime = dto.StartTime;
			requirement.EndTime = dto.EndTime;
			requirement.RequiredAgents = dto.RequiredAgents;

			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var requirement =
				await _context.StaffingRequirements.FindAsync(id);

			if (requirement == null)
			{
				return false;
			}

			_context.StaffingRequirements.Remove(requirement);

			await _context.SaveChangesAsync();

			return true;
		}
	}
}