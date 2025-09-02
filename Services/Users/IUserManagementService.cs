using Models.Entities;

namespace Services.Users;

public interface IUserManagementService
{
    Task<Response<User>> GetUserByIdAsync(string userId);
    Task<Response<string>> AuthenticateUserAsync(string userEmail, string password);
    Task<Response<List<User>>> GetUserFriendsAsync(string userId);
    Task<Response<User>> RegisterUserAsync(User user);
    Task<Response<User>> UpdateUserAsync(string userId, User user);
}