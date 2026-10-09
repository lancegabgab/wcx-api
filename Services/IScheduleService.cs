using wcx_api.DTOs.Inputs;
using wcx_api.DTOs.Outputs;

namespace wcx_api.Services
{
	public interface IScheduleService
	{
		Task<Response<List<ScheduleOutput>>> GetAllAsync();

		Task<Response<ScheduleOutput>> GetByIdAsync(int id);

		Task<Response<ScheduleOutput>> CreateAsync(ScheduleInput input);

		Task<Response<ScheduleOutput>> UpdateAsync(
			int id,
			ScheduleInput input);

		Task<Response<bool>> DeleteAsync(int id);
	}
}
