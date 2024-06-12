using Messager_Project.DTO.User;
using Messager_Project.Model.Enteties;
using Messager_Project.Repository.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PasswordEncryptionService;
using System.Text;

namespace Messager_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <summary>
        /// Zwraca uzytkownika o podanym Id
        /// </summary>
        /// <returns></returns>
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
        /// <summary>
        /// Zwraca emotkę o podanym pseudonimie
        /// </summary>
        /// <returns></returns>
        
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var user = await _userRepository.GetUserByNameAsync(name);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
        /// <summary>
        /// Zwraca wszystkich użytkowników
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userRepository.GetAllUsersThatAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }
        /// <summary>
        /// Dodaje nowego użytkownika
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserInputDto user)
        {
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            PasswordEncryption password = new PasswordEncryption();
            var salt = password.GetSalt();
            var newUser = new User
            {
                Name = user.Name,
                Surname = user.Surname,
                Username = user.Username,
                User_Picture = user.User_Picture,
                PasswordHash = password.GenerateSaltedHash(Encoding.UTF8.GetBytes(user.Password), salt),
                PasswordSalt = salt
            };
            var result = await _userRepository.SaveUserAsync(newUser);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok(newUser);
        }
        /// <summary>
        /// Aktualizuje username, imię, nazwisko, hasło oraz awatar użytkowika o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserInputDto user)
        {
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound();
            PasswordEncryption password = new PasswordEncryption();
            var salt = password.GetSalt();
            existingUser.Username = user.Username;
            existingUser.Name = user.Name;
            existingUser.Surname = user.Surname;
            existingUser.PasswordHash = password.GenerateSaltedHash(Encoding.UTF8.GetBytes(user.Password), salt);
            existingUser.PasswordSalt = salt;
            existingUser.User_Picture = user.User_Picture;
            var result = await _userRepository.SaveUserAsync(existingUser);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
        /// <summary>
        /// Aktualizuję username użytkownika o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpPut("{id}/username")]
        public async Task<IActionResult> UpdateUserUsername(int id, [FromBody] UserInputDto user)
        {
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound();
            existingUser.Username = user.Username;
            var result = await _userRepository.SaveUserAsync(existingUser);
            return Ok();
        }
        /// <summary>
        /// Zmienia imię użytkownika o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpPut("{id}/name")]
        public async Task<IActionResult> UpdateUserName(int id, [FromBody] UserInputDto user)
        {
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound();
            existingUser.Name = user.Name;
            var result = await _userRepository.SaveUserAsync(existingUser);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
        /// <summary>
        /// Zmienia nazwisko użytkownika o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpPut("{id}/surname")]
        public async Task<IActionResult> UpdateUserSurname(int id, [FromBody] UserInputDto user)
        {
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound();
            existingUser.Surname = user.Surname;
            var result = await _userRepository.SaveUserAsync(existingUser);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
        /// <summary>
        /// Zmienia hasło użytkownika o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdateUserPassword(int id, [FromBody] UserInputDto user)
        {
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound();
            PasswordEncryption password = new PasswordEncryption();
            var salt = password.GetSalt();
            existingUser.PasswordHash = password.GenerateSaltedHash(Encoding.UTF8.GetBytes(user.Password), salt);
            existingUser.PasswordSalt = salt;
            var result = await _userRepository.SaveUserAsync(existingUser);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
        /// <summary>
        /// Aktualizuje awatar użytkownika o podanym Id
        /// </summary>
        /// <returns></returns>
        
        [HttpPut("{id}/picture")]
        public async Task<IActionResult> UpdateUserPicture(int id, [FromBody] UserInputDto user)
        {
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound();
            existingUser.User_Picture = user.User_Picture;
            var result=await _userRepository.SaveUserAsync(existingUser);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
        /// <summary>
        /// Usuwa użytkownika o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound();
           var result = await _userRepository.DeleteUserAsync(id);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
    }
}
