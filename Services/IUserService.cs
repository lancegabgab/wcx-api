using wcx_api.DTOs.Outputs;

namespace wcx_api.Services
{
	public interface IUserService
	{
		Task<List<UserOutput>> GetAllAgentsAsync();
	}
}