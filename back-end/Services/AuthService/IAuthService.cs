using Budget_Tracker_WebAPI.DTOs;

namespace Budget_Tracker_WebAPI.Services
{
    public interface IAuthService
    {
        Task ResgisterUserAsync(RegisterUserDTO registerUserDto);
        Task<string> LoginUserAsync(LoginUserDTO loginUserDto);

        UserDetailModel? GetUserDetail(Guid userId);
    }
}
