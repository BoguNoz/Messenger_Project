using Messager_Project.Model;
using Messager_Project.Model.Enteties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Repository.UsersFriends
{
    public class MSUserFriendsRepository : BaseRepository, IMessageEmotesRepository
    {
        public MSUserFriendsRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<UserFriends?> GetUserFriendByIdAsync(int userId, int userFriendId)
        {
            var friend = await DbContext._usersFriends.Where(u => u.User1_ID == userId).SingleOrDefaultAsync(f => f.User2_ID == userFriendId);

            return friend;
        }

        public async Task<UserFriends?> GetRelationIdAsync(int id)
        {
            var relation = await DbContext._usersFriends.SingleOrDefaultAsync(r => r.Relation_ID == id);
            return relation;
        }

        public async Task<List<User>?> GetUserFriendsByNameAsync(int userId, string name)
        {
            var users = await DbContext._users.Where(u => u.Username == name).ToListAsync();

            var inRealtionWithUser = await DbContext._usersFriends.Where(u => u.User1_ID == userId).ToListAsync();

            var friends = new List<User>();

            foreach (var user in users)
            {
                if (inRealtionWithUser.Any(u2 => u2.User2_ID == user.User_ID))
                    friends.Add(user);
            }

            return friends;
        }

        public async Task<List<User>?> GetAllUsersFriendsAsync(int userId)
        {
            var users = await DbContext._usersFriends.Where(u => u.User1_ID == userId).ToListAsync();

            var friends = new List<User>();

            foreach (var user in users)
            {
                var friend = await DbContext._users.SingleOrDefaultAsync(u => u.User_ID == user.User2_ID);
                friends.Add(friend);
            }

            return friends;
        }

        public async Task<bool> SaveRelationAsync(UserFriends relation, User user1, User user2)
        {
            if (relation == null || user1 == null || user2 == null)
                return false;

            //Checking status
            DbContext.Entry(relation).State = relation.Relation_ID == default(int) ? EntityState.Added : EntityState.Modified;

            try
            {
                user1.User_Friends.Add(relation);
                user2.Frinds_With_User.Add(relation);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false; //#Zmień na obiekt
            }

            return true;
        }

        public async Task<bool> DeleteRelationAsync(int id, User user1, User user2)
        {
            var relation = await GetRelationIdAsync(id);


            if (relation == null)
                return true;

            user1.User_Friends.Remove(relation);
            user2.Frinds_With_User.Remove(relation);
            DbContext._usersFriends.Remove(relation);

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
