using System.Web;
using Common.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Services.Adapters;
using Services.Users;

namespace Controlers.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserManagementService _userManagementService;

        public UserController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(string loginUserId)
        {
            var usersResponse = await _userManagementService.GetUserFriendsAsync(loginUserId);
            var usersDto = new List<UserResponseDto>();

            if (usersResponse != null && usersResponse.Status && usersResponse.Object != null)
            {
                foreach (var user in usersResponse.Object)
                {
                    usersDto.Add(await UserAdapter.ConvertModelToResponseDto(user));
                }
            }

            return View(usersDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] UserRequestDto newUser)
        {
            var userModel = await UserAdapter.ConvertRequestDtoToModel(newUser);
            var userResponse = await _userManagementService.RegisterUserAsync(userModel);

            if (!userResponse.Status)
            {
                TempData["Error"] = userResponse.Message ?? "Nie udało się utworzyć użytkownika.";
                return View("UserForm");
            }

            TempData["Message"] = "Użytkownik został utworzony.";
            return View("UserForm");
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction(nameof(Index));

            string decodedId = HttpUtility.UrlDecode(userId);
            var userResponse = await _userManagementService.GetUserByIdAsync(decodedId);
            if (!userResponse.Status)
            {
                TempData["Error"] = userResponse.Message ?? "Nie znaleziono użytkownika.";
                return RedirectToAction(nameof(Index));
            }

            var allUsersResponse = await _userManagementService.GetUserFriendsAsync(userId);
            var usersDto = new List<Common.DTO.User.UserResponseDto>();
            if (allUsersResponse != null && allUsersResponse.Status && allUsersResponse.Object != null)
            {
                foreach (var u in allUsersResponse.Object)
                {
                    usersDto.Add(await UserAdapter.ConvertModelToResponseDto(u));
                }
            }

            ViewBag.AllUsers = usersDto;
            var userDto = await UserAdapter.ConvertModelToResponseDto(userResponse.Object);
            return View(userDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string userId, [FromForm] UserRequestDto newUser)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                TempData["Error"] = "Brak id użytkownika.";
                return RedirectToAction(nameof(Index));
            }

            var userModel = await UserAdapter.ConvertRequestDtoToModel(newUser);
            var userResponse = await _userManagementService.UpdateUserAsync(userId, userModel);

            if (!userResponse.Status)
            {
                TempData["Error"] = userResponse.Message ?? "Nie udało się zaktualizować profilu.";
                return RedirectToAction(nameof(Profile), new { userId });
            }

            TempData["Message"] = "Profil zaktualizowany.";
            return RedirectToAction(nameof(Profile), new { userId });
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
        {
            var authResponse = await _userManagementService.AuthenticateUserAsync(email, password);

            if (!authResponse.Status)
            {
                TempData["Error"] = "Niepoprawny login lub hasło.";
                return View("UserForm");
            }

            TempData["Message"] = "Zalogowano pomyślnie.";
            return View("UserForm");
        }

    }
    
}
