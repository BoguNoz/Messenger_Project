using Models.Entities;
using Repositories.EmoteRepositories;
using Repositories.MessageRepositories;
using Repositories.UserRepositories;

namespace Services.Communication;

public class RavenCommunicationService : ICommunicationService
{
    #region Dependencies
    private readonly DbContext _context;
    private readonly IMessageRepository _messageRepository;
    private readonly IEmoteRepository _emoteRepository;
    private readonly IUserRepository _userRepository;
    private ICommunicationService _communicationServiceImplementation;

    public RavenCommunicationService(DbContext context, IMessageRepository messageRepository, IEmoteRepository emoteRepository, IUserRepository userRepository)
    {
        _context = context;
        _messageRepository = messageRepository;
        _emoteRepository = emoteRepository;
        _userRepository = userRepository;
    }

    #endregion Dependencies
    
    public async Task<Response<List<Message>>> GetByConversationAsync(string senderId, string reciverId, int page, int pageSize = 10)
    {
        return await _messageRepository.GetByConversationAsync(senderId, reciverId, page, pageSize);
    }

    public async Task<Response<List<Message>>> SearchMessagesByContactAsync(string searchText, string userId, int page = 1, int pageSize = 10)
    {
        return await _messageRepository.SearchMessages(searchText, userId, page, pageSize);
    }

    public async Task<Response<List<Message>>> GetUserMessagesAsync(string userId, DateTime? dateFrom, DateTime? dateTo, int page = 1, int pageSize = 10)
    {
        return await _messageRepository.GetUserMessagesByUserId(userId, dateFrom, dateTo, page, pageSize);
    }

    public async Task<int> GetUserMessagesCountAsync(string userId)
    {
        var response = await _messageRepository.GetUserMessageCount(userId);
        return response.Object.Count;
    }

    public async Task<Response<bool>> SendMessagesAsync(string senderId, string[] reciverIds, Message message)
    {
        using var session = await _context.OpenAsyncSession();

        var userIds = reciverIds.Append(senderId).Distinct().ToList();
        var users = await session.LoadAsync<User>(userIds);

        foreach (var userId in userIds)
        {
            if (!users.ContainsKey(userId) || users[userId] == null)
            {
                return new Response<bool>
                {
                    Status = false,
                    Message = $"User {userId} not found.",
                    Object = false
                };
            }
        }

        var saveMessageResponse = await _messageRepository.SaveAsync(message, session);
        if (!saveMessageResponse.Status)
        {
            return new Response<bool>
            {
                Status = false,
                Message = "Message can't be sent.",
                Object = false
            };
        }

        var connectMessageResponse = await _userRepository.AddMessageAsync(userIds.ToArray(), saveMessageResponse.Object.Id, session);
        if (!connectMessageResponse.Status)
        {
            return new Response<bool>
            {
                Status = false,
                Message = "Failed to connect message to users.",
                Object = false
            };
        }

        await session.SaveChangesAsync();

        return new Response<bool>
        {
            Status = true,
            Message = "Message sent.",
            Object = true
        };
    }

    public async Task<Response<List<Emote>>> GetMessageEmotes(string messageId)
    {
        return await _emoteRepository.GetMessageEmotesByIdAsync(messageId);
    }

    public async Task<Response<bool>> AddReactionToMessageAsync(string messageId, string emoteId)
    {
        using var session = await _context.OpenAsyncSession();
        
        var messageResponse = await _messageRepository.GetMessageByIdAsync(messageId);
        messageResponse.Object.Emotes.Add(emoteId);
        
        var saveMessageResponse = await _messageRepository.SaveAsync(messageResponse.Object, session);
        if (!saveMessageResponse.Status)
        {
            return new Response<bool>
            {
                Status = false,
                Message = "Failed to connect reaction to message.",
                Object = false
            };
        }
        
        await session.SaveChangesAsync();

        return new Response<bool>()
        {
            Status = true,
            Object = true
        };
    }

    public async Task<Response<Emote>> AddReactionAsync(Emote emote)
    {
        return await _emoteRepository.SaveAsync(emote);
    }
}