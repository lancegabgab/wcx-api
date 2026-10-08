using wcx_api.DTOs.Outputs;

namespace wcx_api.Services
{
	public interface IAccountService
	{
		Task<Response<UserOutput>> GetProfileAsync(string userId);
	}
}