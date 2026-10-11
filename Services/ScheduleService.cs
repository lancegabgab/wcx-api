
using Microsoft.EntityFrameworkCore;
using wcx_api.Data;
using wcx_api.DTOs.Inputs;
using wcx_api.DTOs.Outputs;
using wcx_api.Models;
using wcx_api.Services;

namespace wcx_api.Services
{
	public class ScheduleService
	{
		private readonly WcxDbContext _context;

		public ScheduleService(WcxDbContext context)
		{
			_context = context;
		}

		public async Task<Response<List<ScheduleOutput>>> GetAllAsync()
		{
			var schedules = await _context.Schedules
				.AsNoTracking()
				.OrderBy(s => s.Date)
				.ThenBy(s => s.StartTime)
				.Select(s => new ScheduleOutput
				{
					Id = s.Id,
					AgentId = s.AgentId,
					Date = s.Date,
					StartTime = s.StartTime,
					EndTime = s.EndTime,
					Status = s.Status
				})
				.ToListAsync();

			return new Response<List<ScheduleOutput>>
			{
				Success = true,
				Message = "Schedules retrieved successfully.",
				Data = schedules
			};
		}

		public async Task<Response<ScheduleOutput>> GetByIdAsync(int id)
		{
			var schedule = await _context.Schedules
				.AsNoTracking()
				.Where(s => s.Id == id)
				.Select(s => new ScheduleOutput
				{
					Id = s.Id,
					AgentId = s.AgentId,
					Date = s.Date,
					StartTime = s.StartTime,
					EndTime = s.EndTime,
					Status = s.Status
				})
				.FirstOrDefaultAsync();

			if (schedule == null)
			{
				return new Response<ScheduleOutput>
				{
					Success = false,
					Message = "Schedule not found.",
					Data = null
				};
			}

			return new Response<ScheduleOutput>
			{
				Success = true,
				Message = "Schedule retrieved successfully.",
				Data = schedule
			};
		}

		public async Task<Response<ScheduleOutput>> CreateAsync(
			ScheduleInput input)
		{
			if (input.EndTime <= input.StartTime)
			{
				return new Response<ScheduleOutput>
				{
					Success = false,
					Message = "End time must be later than start time.",
					Data = null
				};
			}

			var agentExists = await _context.Users
				.AnyAsync(u => u.Id == input.AgentId);

			if (!agentExists)
			{
				return new Response<ScheduleOutput>
				{
					Success = false,
					Message = "The specified agent does not exist.",
					Data = null
				};
			}

			var schedule = new Schedule
			{
				AgentId = input.AgentId,
				Date = input.Date.Date,
				StartTime = input.StartTime,
				EndTime = input.EndTime,
				Status = string.IsNullOrWhiteSpace(input.Status)
					? "Scheduled"
					: input.Status
			};

			_context.Schedules.Add(schedule);
			await _context.SaveChangesAsync();

			var output = new ScheduleOutput
			{
				Id = schedule.Id,
				AgentId = schedule.AgentId,
				Date = schedule.Date,
				StartTime = schedule.StartTime,
				EndTime = schedule.EndTime,
				Status = schedule.Status
			};

			return new Response<ScheduleOutput>
			{
				Success = true,
				Message = "Schedule created successfully.",
				Data = output
			};
		}

		public async Task<Response<ScheduleOutput>> UpdateAsync(
			int id,
			ScheduleInput input)
		{
			var schedule = await _context.Schedules
				.FirstOrDefaultAsync(s => s.Id == id);

			if (schedule == null)
			{
				return new Response<ScheduleOutput>
				{
					Success = false,
					Message = "Schedule not found.",
					Data = null
				};
			}

			if (input.EndTime <= input.StartTime)
			{
				return new Response<ScheduleOutput>
				{
					Success = false,
					Message = "End time must be later than start time.",
					Data = null
				};
			}

			var agentExists = await _context.Users
				.AnyAsync(u => u.Id == input.AgentId);

			if (!agentExists)
			{
				return new Response<ScheduleOutput>
				{
					Success = false,
					Message = "The specified agent does not exist.",
					Data = null
				};
			}

			schedule.AgentId = input.AgentId;
			schedule.Date = input.Date.Date;
			schedule.StartTime = input.StartTime;
			schedule.EndTime = input.EndTime;
			schedule.Status = string.IsNullOrWhiteSpace(input.Status)
				? "Scheduled"
				: input.Status;

			await _context.SaveChangesAsync();

			return new Response<ScheduleOutput>
			{
				Success = true,
				Message = "Schedule updated successfully.",
				Data = new ScheduleOutput
				{
					Id = schedule.Id,
					AgentId = schedule.AgentId,
					Date = schedule.Date,
					StartTime = schedule.StartTime,
					EndTime = schedule.EndTime,
					Status = schedule.Status
				}
			};
		}

		public async Task<Response<bool>> DeleteAsync(int id)
		{
			var schedule = await _context.Schedules
				.FirstOrDefaultAsync(s => s.Id == id);

			if (schedule == null)
			{
				return new Response<bool>
				{
					Success = false,
					Message = "Schedule not found.",
					Data = false
				};
			}

			_context.Schedules.Remove(schedule);
			await _context.SaveChangesAsync();

			return new Response<bool>
			{
				Success = true,
				Message = "Schedule deleted successfully.",
				Data = true
			};
		}
	}
}