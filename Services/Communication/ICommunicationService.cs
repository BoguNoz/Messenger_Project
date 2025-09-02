using Models.Entities;

namespace Services.Communication;

public interface ICommunicationService
{
    Task<Response<List<Message>>> GetByConversationAsync(string senderId, string reciverId, int page, int pageSize = 10);
    Task<Response<List<Message>>> SearchMessagesByContactAsync(string searchText, string userId, int page = 1, int pageSize = 10);
    Task<Response<List<Message>>> GetUserMessagesAsync(string userId, DateTime? dateFrom, DateTime? dateTo, int page = 1, int pageSize = 10);
    Task<int> GetUserMessagesCountAsync(string userId);
    Task<Response<bool>> SendMessagesAsync(string senderId, string[] reciverIds, Message messages);
    Task<Response<List<Emote>>> GetMessageEmotes(string messageId);
    Task<Response<bool>> AddReactionToMessageAsync(string messageId, string emoteId);
    Task<Response<Emote>> AddReactionAsync(Emote emote);
}