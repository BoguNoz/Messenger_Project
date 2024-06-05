using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messager_Project.Model.Enteties;

namespace Messager_Project.Repository.UsersFriends
{
    public interface IMessageEmotesRepository
    {
        Task<UserFriends?> GetUserFriendByIdAsync(int userId, int userFriendId);

        Task<UserFriends?> GetRelationIdAsync(int id);

        Task<List<User>?> GetUserFriendsByNameAsync(int userId, string name);

        Task<List<User>?> GetAllUsersFriendsAsync(int userId);

        Task<bool> SaveRelationAsync(UserFriends relation, User user1, User user2);

        Task<bool> DeleteRelationAsync(int id, User user1, User user2);
    }
}
