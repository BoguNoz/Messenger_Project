using Models;
using Models.Entities;
using Models.Indexes;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Linq;

namespace Repositories.MessageRepositories;

public class RavenMessageRepository : RavenBaseRepository, IMessageRepository
{
    public RavenMessageRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Response<Message>> GetMessageByIdAsync(string messageId)
    {
        using var session = await dbContext.OpenAsyncSession();
        var message = await session.LoadAsync<Message>(messageId);

        return new Response<Message>
        {
            Object = message,
            TotalCount = 1
        };
    }
    
    public async Task<Response<List<Message>>> GetByConversationAsync(string senderId, string reciverId, int page = 1, int pageSize = 10)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);
        
        using var session = await dbContext.OpenAsyncSession();

        QueryStatistics stats = null;
        
        var query = session.Query<Message>()
            .Statistics(out stats)
            .Where(m => m.SenderId == senderId && m.ReciverId == reciverId)
            .OrderBy(m => m.MessageCreation)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var messages = await query.ToListAsync();

        return new Response<List<Message>>
        {
            Object = messages,
            PageNumber = page,
            PageSize = pageSize,
            TotalCount = stats.TotalResults
        };
    }
    
    public async Task<Response<List<Message>>> SearchMessages(string searchText, string userId, int page = 1, int pageSize = 10)
    {
        using var session = await dbContext.OpenAsyncSession();

        QueryStatistics stats = null;

        var results = await session.Query<Message, Message_Search>()
            .Statistics(out stats)
            .Search(x => x.Content, searchText)
            .Where(m => m.SenderId == userId || m.ReciverId == userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new Response<List<Message>>
        {
            Object = results,
            PageNumber = page,
            PageSize = pageSize,
            TotalCount = stats.TotalResults
        };
    }
    
    public async Task<Response<List<Message>>> GetUserMessagesByUserId(string userId, DateTime? dateFrom, DateTime? dateTo, int page = 1, int pageSize = 10)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);
        
        using var session = await dbContext.OpenAsyncSession();
        QueryStatistics stats = null;
        
        var query = session.Query<Message, Message_ByUser>()
            .Statistics(out stats)
            .Where(m => m.SenderId == userId || m.ReciverId == userId);

        if (dateFrom.HasValue)
        {
            query = query.Where(m => m.MessageCreation >= dateFrom.Value);
        }
       
        if (dateTo.HasValue)
        {
            query = query.Where(m => m.MessageCreation <= dateTo.Value);
        }
        
        query = query
            .OrderByDescending(m => m.MessageCreation)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        
        var messages = await query.ToListAsync();

        return new Response<List<Message>>
        {
            Object     = messages,
            PageNumber = page,
            PageSize   = pageSize,
            TotalCount = stats.TotalResults
        };
    }
    
    public async Task<Response<Messages_CountByUser.Result>> GetUserMessageCount(string userId)
    {
        using var session = await dbContext.OpenAsyncSession();

        var result = await session.Query<Messages_CountByUser.Result, Messages_CountByUser>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return new Response<Messages_CountByUser.Result>
            {
                Status = false,
                Message = "No messages found for the user."
            };
        }

        return new Response<Messages_CountByUser.Result>
        {
            Object = result
        };
    }
    
    public async Task<Response<Message>> SaveAsync(Message message, IAsyncDocumentSession session)
    {
        try
        {
            await session.StoreAsync(message);
            await session.SaveChangesAsync();

            return new Response<Message>
            {
                Object = message
            };
        }
        catch (Exception ex)
        {
            return new Response<Message>
            {
                Status = false,
                Message = $"Error saving message: {ex.Message}"
            };
        }
    }

    public async Task<Response<Message>> DeleteAsync(string id, IAsyncDocumentSession session)
    {
        try
        {
            var message = await session.LoadAsync<Message>(id);
            if (message == null)
            {
                return new Response<Message>
                {
                    Status = false,
                    Message = "Emote not found."
                };
            }

            session.Delete(message);
            await session.SaveChangesAsync();

            return new Response<Message>
            {
                Object = message
            };
        }
        catch (Exception ex)
        {
            return new Response<Message>
            {
                Status = false,
                Message = $"Error deleting message: {ex.Message}"
            };
        }
    }
}
