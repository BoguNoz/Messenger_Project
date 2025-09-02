using Messager_Project.Model;
using Messager_Project.Model.Enteties;
using Microsoft.EntityFrameworkCore;
using ResponseModelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbContext = Messager_Project.Model.DbContext;

namespace Messager_Project.Repository.Users
{

    public class MSUserRepository : BaseRepository, IUserRepository
    {
        //BaseRepository Implementation 
        public MSUserRepository(DbContext dbContext) : base(dbContext)
        {

        }

        //Interface Implementation
        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await DbContext._users.SingleOrDefaultAsync(u => u.User_ID.Equals(id));

            return user;
        }

        public async Task<User?> GetUserByNameAsync(string name)
        {
            var userByUsername = await DbContext._users.SingleOrDefaultAsync(u => u.Username.Equals(name));

            return userByUsername;
        }

        //Friends
        public async Task<List<User>?> GetAllUsersThatAsync()
        {
            var allUsers = await DbContext._users.ToListAsync();

            return allUsers;
        }

        public async Task<ResponseModel<User>> SaveUserAsync(User user)
        {
            if (user == null)
                return new ResponseModel<User> { Status = false, Message = "UserRepositories is null", ReferenceObject = null };


            //Generation Of Unique UserRepositories Hash Code 
            var code = DbContext._users.Max(u => u.User_ID) + 1000;
            user.Username += ("#" + code.ToString());

            if (DbContext._users.Any(un => un.Username == user.Username))
                return new ResponseModel<User> { Status = false, Message = "UserRepositories with this username arleady exists", ReferenceObject = user };


            //Checking status
            DbContext.Entry(user).State = user.User_ID == default(int) ? EntityState.Added : EntityState.Modified;

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ResponseModel<User> { Status = false, Message = $"Error: {ex.Message}", ReferenceObject = user };
            }

            return new ResponseModel<User> { Status = true, Message = "UserRepositories saved successfully", ReferenceObject = user };
        }

        public async Task<ResponseModel<User>> DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);

            if (user == null)
                return new ResponseModel<User> { Status = true, Message = "UserRepositories deleted successfully", ReferenceObject = null };

            DbContext._users.Remove(user); //Hard Removal

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ResponseModel<User> { Status = false, Message = $"Error: {ex.Message}", ReferenceObject = user };
            }

            return new ResponseModel<User> { Status = true, Message = "UserRepositories deleted successfully", ReferenceObject = user };
        }
    }
}
