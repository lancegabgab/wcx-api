
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

		public async Task<Response<List<ScheduleOutput>>> GetAllAsync(
			string? agentId,
			DateOnly? dateFrom,
			DateOnly? dateTo,
			TimeSpan? timeFrom,
			TimeSpan? timeTo
		)
		{
			var query = _context.Schedules
				.AsNoTracking()
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(agentId))
				query = query.Where(s => s.AgentId == agentId);

			if (dateFrom.HasValue)
				query = query.Where(s => s.Date >= dateFrom.Value);

			if (dateTo.HasValue)
				query = query.Where(s => s.Date <= dateTo.Value);

			if (timeFrom.HasValue)
				query = query.Where(s => s.StartTime >= timeFrom.Value);

			if (timeTo.HasValue)
				query = query.Where(s => s.EndTime <= timeTo.Value);

			var schedules = await query
				.OrderBy(s => s.Date)
				.ThenBy(s => s.StartTime)
				.Select(s => new ScheduleOutput
				{
					Id = s.Id,
					AgentId = s.AgentId,
					FirstName = s.Agent.FirstName,
					MiddleName = s.Agent.MiddleName,
					LastName = s.Agent.LastName,
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
					FirstName = s.Agent.FirstName,
					MiddleName = s.Agent.MiddleName,
					LastName = s.Agent.LastName,
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

			var hasOverlap = await _context.Schedules
			.AnyAsync(s =>
				s.AgentId == input.AgentId &&
				s.Date == input.Date &&
				s.StartTime < input.EndTime &&
				s.EndTime > input.StartTime
			);

			if (hasOverlap)
			{
				return new Response<ScheduleOutput>
				{
					Success = false,
					Message = "This agent already has a schedule that overlaps with the selected time range on this date.",
					Data = null
				};
			}

			var schedule = new Schedule
			{
				AgentId = input.AgentId,
				Date = input.Date,
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

			var hasOverlap = await _context.Schedules
			.AnyAsync(s =>
				s.Id != id &&
				s.AgentId == input.AgentId &&
				s.Date == input.Date &&
				s.StartTime < input.EndTime &&
				s.EndTime > input.StartTime
			);

			if (hasOverlap)
			{
				return new Response<ScheduleOutput>
				{
					Success = false,
					Message = "This agent already has a schedule that overlaps with the selected time range on this date.",
					Data = null
				};
			}

			schedule.AgentId = input.AgentId;
			schedule.Date = input.Date;
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

		public async Task<Response<List<ScheduleOutput>>> GetMySchedulesAsync(
		string userId,
		DateOnly? dateFrom,
		DateOnly? dateTo,
		TimeSpan? timeFrom,
		TimeSpan? timeTo
		)
		{
			var query = _context.Schedules
				.AsNoTracking()
				.Where(s => s.AgentId == userId);

			if (dateFrom.HasValue)
			{
				query = query.Where(s => s.Date >= dateFrom.Value);
			}

			if (dateTo.HasValue)
			{
				query = query.Where(s => s.Date <= dateTo.Value);
			}

			if (timeFrom.HasValue)
			{
				query = query.Where(s => s.StartTime >= timeFrom.Value);
			}

			if (timeTo.HasValue)
			{
				query = query.Where(s => s.EndTime <= timeTo.Value);
			}

			var schedules = await query
				.OrderBy(s => s.Date)
				.ThenBy(s => s.StartTime)
				.Select(s => new ScheduleOutput
				{
					Id = s.Id,
					AgentId = s.AgentId,
					Date = s.Date,
					StartTime = s.StartTime,
					EndTime = s.EndTime,
					Status = s.Date < DateOnly.FromDateTime(DateTime.Today) ? "Completed" : s.Status
				})
				.ToListAsync();

			return new Response<List<ScheduleOutput>>
			{
				Success = true,
				Message = "Your schedules retrieved successfully.",
				Data = schedules
			};
		}
	}
}