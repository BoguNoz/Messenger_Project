using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Messager_Project.Repository.Messages;
using Messager_Project.DTO.User;
using Messager_Project.Model.Enteties;
using PasswordEncryptionService;
using System.Text;
using Messager_Project.DTO.Message;
using System.Globalization;
using Messager_Project.DTO.Emotes;

namespace Messager_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly Messager_Project.Repository.Messages.IEmotesRepository _messegersRespository;
        public MessagesController (IEmotesRepository messegersRespository)
        {
            _messegersRespository = messegersRespository;
        }
        /// <summary>
        /// Zwraca wszystkie wiadomości wysłane przez użytkowników
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _messegersRespository.GetAllMessagesAsync();
            return Ok(messages);
        }
        /// <summary>
        /// Zwraca wszystkie wiadomości wysłane przez użytkownika o podanym Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id={Id}")]
        public async Task<IActionResult> GetMessageById(int Id)
        {
            var message = await _messegersRespository.GetMessageByIdAsync(Id);
            if (message == null)
            {
                return(NotFound());
            }
            return Ok(message);
        }
        /// <summary>
        /// Zwraca wszystkie wiadomości wysłane przez użytkowika o senderId, do użytkownika o reciverId
        /// </summary>
        /// <returns></returns>
        [HttpGet("/getAllMessageRecivedFromById/senderId={senderId}reciverId={reciverId}")]
        public async Task<IActionResult> GetAllMessageRecivedFromById(int reciverId, int senderId)
        {
            var message = await _messegersRespository.GetAllMessageRecivedFromByIdAsync(reciverId, senderId);
            if (message == null)
            {
                return (NotFound());
            }
            return Ok(message);
        }
        /// <summary>
        /// Zwraca wszystkie wiadomości otrzymane przez użytkowika o reciverId, wysłąne przez użytkownika o senderId
        /// </summary>
        /// <returns></returns>
        [HttpGet("/getAllMessageSendToByIdAsync/senderId={senderId}reciverId={reciverId}")]
        public async Task<IActionResult> GetAllMessageSendToById(int reciverId, int senderId)
        {
            var message = await _messegersRespository.GetAllMessageSendToByIdAsync(reciverId, senderId);
            if (message == null)
            {
                return (NotFound());
            }
            return Ok(message);
        }
        /// <summary>
            /// Dodaje nową wiadomość od użytkownika o creatorId do reciverId o treści Message_Content
        /// </summary>
        /// <returns></returns>
        [HttpPost("/addMessage/creatorId={creatorId}reciverId={reciverId}")]
        public async Task<IActionResult> Post(int creatorId, int reciverId, [FromBody] MessageDto messageDto)
        {
            var message = new Message
            {
                Message_Creation = DateTime.Now,
                Message_Content = messageDto.Message_Content,
                Sender_ID = creatorId,
                Reciver_ID = reciverId
            };
            var result = await _messegersRespository.SaveRelationAsync(message);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
        /// <summary>
        /// Zmienia treść wiadomości o podanym id
        /// </summary>
        /// <returns></returns>
        [HttpPut("changeMessageContent/id={Id}")]
        public async Task<IActionResult> ChangeMessageContent(int Id, [FromBody] MessageDto messageDto)
        {
            var message = await _messegersRespository.GetMessageByIdAsync(Id);
            if (message == null)
                return BadRequest();
            message.Message_Content = messageDto.Message_Content;
            var result = await _messegersRespository.SaveRelationAsync(message);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
        /// <summary>
        /// Usuwa wiadomość o podanym Id
        /// </summary>
        /// <returns></returns>
        [HttpDelete("/deleteMessage/id={id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _messegersRespository.GetMessageByIdAsync(id);
            if (message == null)
                return NotFound();
            var result = await _messegersRespository.DeleteRelationAsync(id);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
    }
}
