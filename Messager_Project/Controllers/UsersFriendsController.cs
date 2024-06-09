using Messager_Project.DTO.UserFriends;
using Messager_Project.Model.Enteties;
using Messager_Project.Repository.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Messager_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersFriendsController : ControllerBase
    {
        private readonly Messager_Project.Repository.UsersFriends.IMessageEmotesRepository _usersFriendsRespository;
        private readonly IUserRepository _userRepository;

        public UsersFriendsController(Messager_Project.Repository.UsersFriends.IMessageEmotesRepository usersFriendsRespository, IUserRepository userRepository)
        {
            _usersFriendsRespository = usersFriendsRespository;
            _userRepository = userRepository;
        }
        /// <summary>
        /// Zwraca znajomych użytkownika o podanym Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("userId={userId}")]
        public async Task<IActionResult> GetUsersFriends(int userId)
        {
            var friends = await _usersFriendsRespository.GetAllUsersFriendsAsync(userId);
            if (friends == null)
                return NotFound();
            return Ok(friends);
        }
        /// <summary>
        /// Dodaje do znajomych użytkownika o id userFriendId użytkownikowi o identyfikatorze userId
        /// </summary>
        /// <returns></returns>
        [HttpPost("userId={userId}")]
        public async Task<IActionResult> AddUsersFriend(int userId, int userFriendId)
        {
            var user1 = await _userRepository.GetUserByIdAsync(userId);
            var user2 = await _userRepository.GetUserByIdAsync(userFriendId);

            var userFriendsRelation = new UserFriends
            {
                User1_ID = user1.User_ID,
                User2_ID = user2.User_ID
            };

            var result = await _usersFriendsRespository.SaveRelationAsync(userFriendsRelation, user1, user2);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
        /// <summary>
        /// Usuwa użytkownika userFirendId ze znajomych użytkownika userId
        /// </summary>
        /// <returns></returns>
        [HttpDelete("userId={userId}userFriendId={userFriendId}")]
        public async Task<IActionResult> DeleteUserFriend(int userId, int userFriendId)
        {
            var user1 = await _userRepository.GetUserByIdAsync(userId);
            if (user1 == null)
                return NotFound();

            var user2 = await _userRepository.GetUserByIdAsync(userFriendId);
            if (user2 == null)
                return NotFound();

            var relation = await _usersFriendsRespository.GetRelationIdByUser1User2(userId, userFriendId);

            var userFriendsRelation = await _usersFriendsRespository.GetRelationIdAsync(relation.Relation_ID);
            if (userFriendsRelation == null) 

                return NotFound();
            var result = await _usersFriendsRespository.DeleteRelationAsync(userFriendsRelation.Relation_ID, user1, user2);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
    }
}
