using Common;
using Models.Entities;
using Repositories.UserRepositories;
using Services.Authentication;

namespace Services.Users;

public class RavenUserManagementService : IUserManagementService
{
    #region Dependencies
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;
    #endregion Dependencies

    public RavenUserManagementService(IUserRepository userRepository, IAuthenticationService authenticationService)
    {
        _userRepository = userRepository;
        _authenticationService = authenticationService;
    }
    
    public Task<Response<User>> GetUserByIdAsync(string userId)
    {
       return _userRepository.GetByIdAsync(userId);
    }

    public async Task<Response<string>> AuthenticateUserAsync(string userEmail, string password)
    {
        var userResponse = await _userRepository.GetByEmailAsync(userEmail);
        if (!userResponse.Status)
        {
            return new Response<string>()
            {
                Status = false,
                Object = string.Empty, 
            };
        }
        var user = userResponse.Object;

        HashFunction hashFunction = new HashFunction();
        if(!hashFunction.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
        {
            return new Response<string>
            {
                Status = false,
                Object = string.Empty,
            };
        }

        var authenticationResponse = await _authenticationService.GenerateTokenByUserIdAsync(user.Id);
        if (!authenticationResponse.Status)
        {
            return new Response<string>
            {
                Status = false,
                Object = string.Empty,
                Message = authenticationResponse.Message
            };
        }

        return new Response<string>
        {
            Object = authenticationResponse.Object,
        };
    }

    public Task<Response<List<User>>> GetUserFriendsAsync(string userId)
    {
        return _userRepository.GetUserFriendsAsync(userId);
    }

    public async Task<Response<User>> RegisterUserAsync(User user)
    {
        
        var checkForClone = await _userRepository.GetByEmailAsync(user.Email);
        if(!checkForClone.Status)
        {
            return new Response<User>
            {
                Status = false,
                Object = null,
            };
        }
        
        var userResponse = await _userRepository.SaveAsync(user);
        if(!userResponse.Status)
        {
            return new Response<User>
            {
                Status = false,
                Object = null,
                Message = userResponse.Message
            };
        }

        return new Response<User>
        {
            Object = userResponse.Object,
        };
    }

    public async Task<Response<User>> UpdateUserAsync(string userId, User updatedUser)
    {
        var userResponse = await _userRepository.GetByIdAsync(userId);
        if (!userResponse.Status)
        {
            return new Response<User>
            {
                Object = null,
                Status = false,
            };
        }
        var oldUser = userResponse.Object;

        if (string.IsNullOrWhiteSpace(updatedUser.Name))
        {
            oldUser.Name = updatedUser.Name;
        }

        if (string.IsNullOrWhiteSpace(updatedUser.Email))
        {
            oldUser.Email = updatedUser.Email;
        }

        foreach (var friend in updatedUser.Friends)
        {
            oldUser.Friends.Append(friend);
        }

        var response = await _userRepository.SaveAsync(updatedUser);
        if (!response.Status)
        {
            return new Response<User>
            {
                Object = null,
                Status = false,
                Message = response.Message
            };
        }

        return new Response<User>
        {
            Object = response.Object,
        };
    }
}