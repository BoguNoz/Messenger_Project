using Common;
using Models;
using Models.Entities;
using Models.Indexes;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Linq; // potrzebne do Query<T>()

namespace Repositories.EmoteRepositories;

public class RavenEmoteRepository : RavenBaseRepository, IEmoteRepository
{
    public RavenEmoteRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Response<List<Emote>>> GetSetByIdAsync(string id, int page = 1, int pageSize = 10)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);
        
        using var session = await dbContext.OpenAsyncSession();

        QueryStatistics stats = null;

        var query = session.Query<Emote>()
            .Statistics(out stats)
            .Where(e => e.Id == id)
            .OrderBy(e => e.Id)                            
            .Skip((page - 1) * pageSize)                  
            .Take(pageSize);                               

        var emotes = await query.ToListAsync();

        return new Response<List<Emote>>
        {
            Object     = emotes,
            PageNumber = page,
            PageSize   = pageSize,
        };
    }

    public async Task<Response<List<Emote>>> GetMessageEmotesByIdAsync(string messageId)
    {
        using var session = await dbContext.OpenAsyncSession();

        var query = await session
            .Query<Emote_ByMessageIndex.Result, Emote_ByMessageIndex>()
            .Where(r => r.MessageId == messageId)
            .Select(r => r.EmoteId)
            .ToListAsync();
        
        var emoteDict = await session.LoadAsync<Emote>(query);

        var emotes = query
            .Where(id => emoteDict.ContainsKey(id))
            .Select(id => emoteDict[id])
            .ToList();

        return new Response<List<Emote>>
        {
            Object = emotes,
            TotalCount = emotes.Count
        };
    }

    public async Task<Response<Emote>> SaveAsync(Emote emote)
    {
        try
        {
            using var session = await dbContext.OpenAsyncSession();

            await session.StoreAsync(emote);
            await session.SaveChangesAsync();

            return new Response<Emote>
            {
                Object = emote
            };
        }
        catch (Exception ex)
        {
            return new Response<Emote>
            {
                Status = false,
                Message = $"Error saving emote: {ex.Message}"
            };
        }
    }

    public async Task<Response<Emote>> DeleteAsync(string id)
    {
        try
        {
            using var session = await dbContext.OpenAsyncSession();

            var emote = await session.LoadAsync<Emote>(id);
            if (emote == null)
            {
                return new Response<Emote>
                {
                    Status = false,
                    Message = "Emote not found."
                };
            }

            session.Delete(emote);
            await session.SaveChangesAsync();

            return new Response<Emote>
            {
                Object = emote
            };
        }
        catch (Exception ex)
        {
            return new Response<Emote>
            {
                Status = false,
                Message = $"Error deleting emote: {ex.Message}"
            };
        }
    }
}
