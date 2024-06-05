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
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);

        Task<List<User>?> GetUserByNameAsync(string name);

        Task<List<User>?> GetAllUsersThatAsync();

        Task<bool> SaveUserAsync(User user);

        Task<bool> DeleteUserAsync(int id);

    }
}
