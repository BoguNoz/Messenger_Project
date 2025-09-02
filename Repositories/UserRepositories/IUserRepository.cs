using Common;
using Models.Entities;
using Raven.Client.Documents.Session;

namespace Repositories.UserRepositories;

public interface IUserRepository
{
    Task<Response<User>> GetByIdAsync(string id);
    Task<Response<User>> GetByEmailAsync(string email);
    Task<Response<bool>> AddMessageAsync(string[] userIds, string messageId, IAsyncDocumentSession session);
    Task<Response<List<User>>> GetUserFriendsAsync(string id);
    Task<Response<User>> SaveAsync(User user);
    Task<Response<User>> DeleteAsync(string id);
}