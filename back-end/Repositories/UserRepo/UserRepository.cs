using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker_WebAPI.Repositories.UserRepo
{
    public class UserRepository(DbExpenseTrackerContext db) : IUserRepository
    {
        public async  Task AddUserAsync(UserMaster user)
        {

            try
            {
                user.UserMasterId = Guid.NewGuid();
                user.CreatedAt = DateTime.UtcNow;
             
                await db.UserMasters.AddAsync(user);
                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error add new user. Please try again.\"", ex);
            }
        }
        
        public async Task<UserMaster?> GetUserByEmailAsync(string email)
        {
            try
            {
                var user =  await db.UserMasters.Include(i=>i.CurrencyMaster).Where(i => i.UserEmail == email).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error getting user. Please try again.\"", ex);
            }
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            try
            {
                var exists = await db.UserMasters.AnyAsync(u => u.UserEmail == email);
                return exists;
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error getting user. Please try again.\"", ex);
            }
        }

        public UserDetailModel? GetUserDetail(Guid userId)
        {
            try
            {
                var user = db.UserMasters.Include(i=>i.UserRole).Include(i=>i.CurrencyMaster).FirstOrDefault(i => i.UserMasterId == userId);

                if(user != null)
                {
                    var userDetailsModel = new UserDetailModel
                    {
                        UserMasterId = user.UserMasterId,
                        UserEmail = user.UserEmail,
                        FirstName =  user.FirstName,
                        LastName =  user.LastName,
                        UserRoleId =  user.UserRoleId,
                        CurrencyMasterId = user.CurrencyMasterId  ?? null,
                        CurrencySymbol =  user.CurrencyMaster?.CurrencySymbol ?? null, 
                        CurrencyCode =  user.CurrencyMaster?.CurrencyCode ?? null,
                        UserRole = user.UserRole?.UserRole
                    };

                    return userDetailsModel;

                }
                return null;

            }
            catch(Exception ex)
            {
                throw new Exception("\"There was an error getting user. Please try again.\"", ex);
            }
        }
    }
}
