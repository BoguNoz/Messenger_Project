using Models;
using Models.Entities;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands.Batches;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;

namespace Repositories.UserRepositories;

 public class RavenUserRepository : RavenBaseRepository, IUserRepository
{
    public RavenUserRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Response<User>> GetByIdAsync(string id)
    {
        using var session = await dbContext.OpenAsyncSession();
        var user = await session.LoadAsync<User>(id);
        if (user == null)
        {
            return new Response<User>
            {
                Object = new User()
            };
        }
        
        return new Response<User>
        {
            Object = user,
            TotalCount = 1,
        };
    }

    public async Task<Response<User>> GetByEmailAsync(string email)
    {
        using var session = await dbContext.OpenAsyncSession();
        var user = await session.Query<User>().FirstOrDefaultAsync(u => u.Email == email);
        
        return new Response<User>
        {
            Object = user,
            TotalCount = 1,
        };
    }

    public async Task<Response<bool>> AddMessageAsync(string[] userIds, string messageId, IAsyncDocumentSession session)
    {
        foreach (var userId in userIds)
        {
            var user = await session.LoadAsync<User>(userId);
            if (user == null)
            {
                return new Response<bool>
                {
                    Status = false,
                    Message = $"User {userId} not found.",
                    Object = false
                };
            }

            user.Messages ??= new List<string>();

            if (!user.Messages.Contains(messageId))
            {
                user.Messages.Add(messageId);
            }
        }

        return new Response<bool>
        {
            Status = true,
            Message = "Messages linked to users.",
            Object = true
        };
        
    }

    public async Task<Response<List<User>>> GetUserFriendsAsync(string id)
    {
        using var session = await dbContext.OpenAsyncSession();
        var user = await session
            .Include<User>(u => u.Friends)
            .LoadAsync<User>(id);

        if (user == null)
        {
            return new Response<List<User>>
            {
                Status = false,
                Message = "User not found.",
                Object = new List<User>()
            };
        }

        var friendsList = user.Friends ?? new List<string>(); // zabezpieczenie przed null

        if (!friendsList.Any())
        {
            return new Response<List<User>>
            {
                Status = true,
                Object = new List<User>(),
                TotalCount = 0
            };
        }

        var friendsDict = await session.LoadAsync<User>(friendsList.ToArray());

        var friends = friendsList
            .Where(fid => friendsDict.ContainsKey(fid) && friendsDict[fid] != null)
            .Select(fid => friendsDict[fid])
            .ToList();

        return new Response<List<User>>
        {
            Object = friends,
            TotalCount = friends.Count,
            Status = true
        };
    }


    public async Task<Response<User>> SaveAsync(User user)
    {
        try
        {
            using var session = await dbContext.OpenAsyncSession();
            await session.StoreAsync(user);
            await session.SaveChangesAsync();

            return new Response<User>
            {
                Object = user
            };
        }
        catch (Exception ex)
        {
            return new Response<User>
            {
                Status  = false,
                Message = $"Error saving user: {ex.Message}"
            };
        }
    }

    public async Task<Response<User>> DeleteAsync(string id)
    {
        try
        {
            using var session = await dbContext.OpenAsyncSession();
            var user = await session.LoadAsync<User>(id);
            if (user == null)
            {
                return new Response<User>
                {
                    Status  = false,
                    Message = "User not found."
                };
            }

            session.Delete(user);
            await session.SaveChangesAsync();

            return new Response<User>
            {
                Object = user
            };
        }
        catch (Exception ex)
        {
            return new Response<User>
            {
                Status  = false,
                Message = $"Error deleting user: {ex.Message}"
            };
        }
    }
}