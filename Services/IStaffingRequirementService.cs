using wcx_api.DTOs.Outputs;
using wcx_api.DTOs.Inputs;

namespace wcx_api.Services
{
	public interface IStaffingRequirementService
	{
		Task<List<StaffingRequirementOutput>> GetAllAsync();

		Task<StaffingRequirementOutput?> GetByIdAsync(int id);

		Task<StaffingRequirementOutput> CreateAsync(
			StaffingRequirementInput dto);

		Task<bool> UpdateAsync(
			int id,
			StaffingRequirementInput dto);

		Task<bool> DeleteAsync(int id);
	}
}
