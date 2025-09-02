using System.Runtime.InteropServices.JavaScript;
using System.Web;
using Common.DTO.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Services.Adapters;
using Services.Communication;

namespace Controlers.Controllers;

[Route("communication/")]
[ApiController]
public class CommunicationController : Controller
{
    private readonly ICommunicationService _communicationService;
    
    public CommunicationController(ICommunicationService communicationService)
    {
        _communicationService = communicationService;
    }

    #region Communication
    [Authorize]
    [HttpGet("sender/{senderId}/receiver/{receiverId}")]
    public async Task<IActionResult> GetByConversationAsync(string senderId, string receiverId, [FromQuery] int page, [FromQuery] int pageSize = 10)
    {
        var decodedIdSender = HttpUtility.UrlDecode(senderId);
        var decodedIdReceiver = HttpUtility.UrlDecode(receiverId);
        var messageResponse = await _communicationService.GetByConversationAsync(decodedIdSender, decodedIdReceiver, page, pageSize);
        if (!messageResponse.Status)
        {
            return BadRequest(new { message = messageResponse.Message });
        }
        
        var eventDtoResponse = new List<MessageResponseDto>();
        foreach (var message in messageResponse.Object)
        {
            eventDtoResponse.Add(await MessageAdapter.ConvertModelToResponseDto(message));
        }
        
        return Ok(eventDtoResponse);
    }
    
    [Authorize]
    [HttpGet("content/{searchText}/user/{userId}")]
    public async Task<IActionResult> SearchMessagesByContactAsync(string searchText, string userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        string decodedId = HttpUtility.UrlDecode(userId);
        var messageResponse = await _communicationService.SearchMessagesByContactAsync(searchText, decodedId, page, pageSize);
        if (!messageResponse.Status)
        {
            return BadRequest(new { message = messageResponse.Message });
        }
        
        var eventDtoResponse = new List<MessageResponseDto>();
        foreach (var message in messageResponse.Object)
        {
            eventDtoResponse.Add(await MessageAdapter.ConvertModelToResponseDto(message));
        }
        
        return Ok(eventDtoResponse);
    }

    [Authorize]
    [HttpGet("user/{senderId}")]
    public async Task<IActionResult> GetUserMessagesAsync(string senderId, [FromQuery]  DateTime? dateFrom, [FromQuery] DateTime? dateTo,
        [FromQuery]  int page = 1, [FromQuery] int pageSize = 10)
    {
        var decodedIdSender = HttpUtility.UrlDecode(senderId);
        var messageResponse = await _communicationService.GetUserMessagesAsync(decodedIdSender, dateFrom, dateTo, page, pageSize);
        if (!messageResponse.Status)
        {
            return BadRequest(new { message = messageResponse.Message });
        }
        
        var eventDtoResponse = new List<MessageResponseDto>();
        foreach (var message in messageResponse.Object)
        {
            eventDtoResponse.Add(await MessageAdapter.ConvertModelToResponseDto(message));
        }
        
        return Ok(eventDtoResponse);
    }

    [Authorize]
    [HttpGet("user/{senderId}/message-count")]
    public async Task<IActionResult> GetUserMessagesCountAsync(string userId)
    {
        string decodedId = HttpUtility.UrlDecode(userId);
        var messageResponse = await _communicationService.GetUserMessagesCountAsync(decodedId);
        return Ok(messageResponse);
    }

    [Authorize]
    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessagesAsync([FromBody] MessageRequestDto message)
    {
        var decodedIdSender = HttpUtility.UrlDecode(message.SenderId);
        var decodedIdReceiver = HttpUtility.UrlDecode(message.ReciverId);
        var messsage = await MessageAdapter.ConvertRequestDtoToModel(message);
        var sendResponse = await _communicationService.SendMessagesAsync(decodedIdSender, [decodedIdReceiver], messsage);
        
        return Ok(sendResponse.Status);
    }
    #endregion Communication
    
    #region Reactions

    [Authorize]
    [HttpGet("{messageId}/reactions")]
    public async Task<IActionResult> GetMessageEmotes(string messageId)
    {
        string decodedId = HttpUtility.UrlDecode(messageId);
        var messageResponse = await _communicationService.GetMessageEmotes(decodedId);
        return Ok(messageResponse.Object);
    }

    [Authorize]
    [HttpPost("{messageId}/reactions/{emoteId}")]
    public async Task<IActionResult> AddReactionToMessageAsync(string messageId, string emoteId)
    {
        var decodedIdMessage = HttpUtility.UrlDecode(messageId);
        var decodedIdEmote = HttpUtility.UrlDecode(emoteId);
        var messageResponse = await _communicationService.AddReactionToMessageAsync(decodedIdMessage, decodedIdEmote);
        return Ok(messageResponse);
    }
    
    [Authorize]
    [HttpPost("reactions")]
    public async Task<IActionResult> AddReactionToMessageAsync([FromBody] Emote emote)
    {
        var messageResponse = await _communicationService.AddReactionAsync(emote);
        return Ok(messageResponse.Object);
    }
    #endregion Reactions
}