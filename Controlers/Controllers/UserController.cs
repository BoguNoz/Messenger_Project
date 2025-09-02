using System.Web;
using Common.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Adapters;
using Services.Users;

namespace Controlers.Controllers;

[Route("users/")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserManagementService _userManagementService;
    
    public UserController(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }
    
    #region User
    [Authorize]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        string decodedId = HttpUtility.UrlDecode(userId);
        var userResponse = await _userManagementService.GetUserByIdAsync(decodedId);
        if (!userResponse.Status)
        {
            return BadRequest(new { message = userResponse.Message });
        }

        var eventDtoResponse = await UserAdapter.ConvertModelToResponseDto(userResponse.Object);
        return Ok(eventDtoResponse);
    }
    
    [Authorize]
    [HttpGet("{userId}/friends")]
    public async Task<IActionResult> GetUserFriends(string userId)
    {
        string decodedId = HttpUtility.UrlDecode(userId);
        var userResponse = await _userManagementService.GetUserFriendsAsync(decodedId);
        if (!userResponse.Status)
        {
            return BadRequest(new { message = userResponse.Message });
        }

        var eventDtoResponse = new List<UserResponseDto>();
        foreach (var user in userResponse.Object)
        {
            eventDtoResponse.Add(await UserAdapter.ConvertModelToResponseDto(user));
        }
        return Ok(eventDtoResponse);
    }
    
    [Authorize]
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserRequestDto newUser)
    {
        string decodedId = HttpUtility.UrlDecode(userId);
        var user = await UserAdapter.ConvertRequestDtoToModel(newUser);
        var userResponse = await _userManagementService.UpdateUserAsync(decodedId, user);
        if (!userResponse.Status)
        {
            return BadRequest(new { message = userResponse.Message });
        }

        var userDtoResponse = await UserAdapter.ConvertModelToResponseDto(userResponse.Object);
        return CreatedAtAction(nameof(GetUserById), new { userId = userDtoResponse.Id }, userDtoResponse);
    }
    #endregion User
    
    #region Authentication
    [HttpPost]
    public async Task<IActionResult> RegisterNewUser([FromBody] UserRequestDto userRequestDto)
    {
        var userModel = await UserAdapter.ConvertRequestDtoToModel(userRequestDto);
        userModel.Messages = new List<string>();
        var userResponse = await _userManagementService.RegisterUserAsync(userModel);
        if (!userResponse.Status)
        {
            return BadRequest(new { message = userResponse.Message });
        }

        var userDtoResponse = await UserAdapter.ConvertModelToResponseDto(userResponse.Object);
        return CreatedAtAction(nameof(GetUserById), new { userId = userDtoResponse.Id }, userDtoResponse);
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateUser([FromBody] UserCredentialsDto userCredentialDto)
    {
        var userResponse = await _userManagementService.AuthenticateUserAsync(userCredentialDto.Email, userCredentialDto.Password);
        if (!userResponse.Status)
        {
            return BadRequest(new { message = userResponse.Message });
        }
        
        return Ok(userResponse.Object);
    }
    #endregion Authentication
}