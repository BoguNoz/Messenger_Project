﻿using Messager_Project.DTO.MessageEmotes;
using Messager_Project.Model.Enteties;
using Messager_Project.Repository.Emote;
using Messager_Project.Repository.MessageEmote;
using Messager_Project.Repository.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Messager_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageEmotesController : ControllerBase
    {
        private readonly IMessageEmotesRepository _messageEmoteRepository;
        private readonly Repository.Emote.IEmotesRepository _emotesRespository;
        private readonly Repository.Messages.IEmotesRepository _messagesRespository;
        public MessageEmotesController (IMessageEmotesRepository messageEmotesRepository, Repository.Emote.IEmotesRepository emotesRepository, Repository.Messages.IEmotesRepository messagesRespository)
        {
            _messageEmoteRepository = messageEmotesRepository;
            _emotesRespository = emotesRepository;
            _messagesRespository = messagesRespository;
        }
        /// <summary>
        /// Zwraca wszystkie emotki przypisane do wszystkich wiadomości
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMessageEmotes()
        {
            var messagesEmotes = await _messageEmoteRepository.GetAllMessageEmotesAsync();
            return Ok(messagesEmotes);
        }
        /// <summary>
        /// Zwraca wszystkie emotki przypisane do wiadomości o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetMessageEmotesById(int Id)
        {
            var messagesEmotes = await _messageEmoteRepository.GetMessageEmotesByIdAsync(Id);
            return Ok(messagesEmotes);
        }
        /// <summary>
        /// Dodaje emotkę do wiadomości
        /// </summary>
        /// <returns></returns>

        [HttpPost("{relationId}/{messageId}/{emoteId}")]
        public async Task<IActionResult> AddMessageEmotes(int relationId, int messageId, int emoteId)
        {
            var message = await _messagesRespository.GetMessageByIdAsync(messageId);
            var emote = await _emotesRespository.GetEmotesByIdAsync(emoteId);
            var messageEmotes = new MessageEmotes
            {
                Emote_ID = emote.Emote_ID,
                Message_ID = message.Message_ID,
            };
            var result = await _messageEmoteRepository.SaveRelationshipAsync(messageEmotes, emote, message);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
        /// <summary>
        /// Usuwa emotkę z wiadomości
        /// </summary>
        /// <returns></returns>

        [HttpDelete("{relationId}/{messageId}/{emoteId}")]
        public async Task<IActionResult> DeleteMessageEmote(int relationId, int messageId, int emoteId)
        {
            var message = await _messagesRespository.GetMessageByIdAsync(messageId);
            var emote = await _emotesRespository.GetEmotesByIdAsync(emoteId);
            var result = await _messageEmoteRepository.DeleteRelationshipAsync(relationId, emote, message);
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
    }
}
