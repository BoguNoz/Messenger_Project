using Messager_Project.Model;
using Messager_Project.Model.Enteties;
using Microsoft.EntityFrameworkCore;
using ResponseModelService;
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

        Task<User?> GetUserByNameAsync(string name);

        Task<List<User>?> GetAllUsersThatAsync();

        Task<ResponseModel<User>> SaveUserAsync(User user);

        Task<ResponseModel<User>> DeleteUserAsync(int id);

    }
}
