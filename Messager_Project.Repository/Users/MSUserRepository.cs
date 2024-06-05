using Messager_Project.Model;
using Messager_Project.Model.Enteties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Repository.Users
{

    public class MSUserRepository : BaseRepository, IUserRepository
    {
        //BaseRepository Implementation 
        public MSUserRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        //Interface Implementation
        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await DbContext._users.SingleOrDefaultAsync(u => u.User_ID.Equals(id));

            return user;
        }

        public async Task<List<User>?> GetUserByNameAsync(string name)
        {
            var userByUsername = await DbContext._users.Where(u => u.Username.Equals(name)).ToListAsync();

            return userByUsername;
        }

        //Friends
        public async Task<List<User>?> GetAllUsersThatAsync()
        {
            var allUsers = await DbContext._users.ToListAsync();

            return allUsers;
        }

        public async Task<bool> SaveUserAsync(User user)
        {
            if (user == null)
                return false;

            //Checking status
            DbContext.Entry(user).State = user.User_ID == default(int) ? EntityState.Added : EntityState.Modified;

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false; //#Zmień na obiekt
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);

            if (user == null)
                return true;

            DbContext._users.Remove(user); //Hard Removal

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
