using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker_WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Db15765Context db;

        public UserRepository(Db15765Context _db) 
        {
        
            this.db = _db;
        
        }

        public async  Task AddUserAsync(UserMaster user)
        {

            try
            {
                user.UserMasterId = Guid.NewGuid();
                user.CreatedAt = DateTime.UtcNow;
             
                await db.UserMasters.AddAsync(user);
                db.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error add new user. Please try again.\"", ex);
            }
        }

        public async Task<UserMaster> GetUserByEmailAsync(string email)
        {
            try
            {
                return await db.UserMasters.Where(i => i.UserEmail == email).FirstOrDefaultAsync();
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
                var user = db.UserMasters.Include(i=>i.UserRole).FirstOrDefault(i => i.UserMasterId == userId);

                if(user != null)
                {
                    var userDetailsModel = new UserDetailModel
                    {
                        UserMasterId = user.UserMasterId,
                        UserEmail = user.UserEmail,
                        FirstName =  user.FirstName,
                        LastName =  user.LastName,
                        UserRoleId =  user.UserRoleId,
                        UserRole = user.UserRole.UserRole
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
