using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;

namespace Budget_Tracker_WebAPI.Repositories
{
    public interface IUserRepository
    {
        Task<UserMaster> GetUserByEmailAsync(string email);
        Task AddUserAsync(UserMaster user);
        Task<bool> IsEmailExistsAsync(string email);
        UserDetailModel GetUserDetail(Guid userId);
    }
}
