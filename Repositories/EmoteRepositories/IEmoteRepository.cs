using Common;
using Models.Entities;

namespace Repositories.EmoteRepositories;

public interface IEmoteRepository
{
    Task<Response<List<Emote>>> GetSetByIdAsync(string id, int page = 1, int pageSize = 10);
    Task<Response<List<Emote>>> GetMessageEmotesByIdAsync(string messageId);
    Task<Response<Emote>> SaveAsync(Emote emote);
    Task<Response<Emote>> DeleteAsync(string id);
}