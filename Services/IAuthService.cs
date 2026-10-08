using wcx_api.DTOs.Inputs;
using wcx_api.DTOs.Outputs;

namespace wcx_api.Services
{
	public interface IAuthService
	{
		Task<Response<UserOutput>> RegisterAsync(UserInput input);
		Task<Response<LoginOutput>> LoginAsync(LoginInput input);
	}
}
