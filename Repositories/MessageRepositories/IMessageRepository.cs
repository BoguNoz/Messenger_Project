using Common;
using Models.Entities;
using Models.Indexes;
using Raven.Client.Documents.Session;

namespace Repositories.MessageRepositories;

public interface IMessageRepository
{
    Task<Response<Message>> GetMessageByIdAsync(string messageId);
    Task<Response<List<Message>>> GetByConversationAsync(string senderId, string reciverId, int page = 1, int pageSize = 10);
    Task<Response<List<Message>>> SearchMessages(string searchText, string userId, int page = 1, int pageSize = 10);
    Task<Response<List<Message>>> GetUserMessagesByUserId(string userId, DateTime? dateFrom, DateTime? dateTo, int page = 1, int pageSize = 10);
    Task<Response<Messages_CountByUser.Result>> GetUserMessageCount(string userId);
    Task<Response<Message>> SaveAsync(Message message, IAsyncDocumentSession session);
    Task<Response<Message>> DeleteAsync(string id, IAsyncDocumentSession session);
}