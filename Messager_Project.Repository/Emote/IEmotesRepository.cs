using Messager_Project.Model.Enteties;
using ResponseModelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Repository.Emote
{
    public interface IEmotesRepository
    {
        Task<Emotes?> GetEmotesByIdAsync(int id);

        Task<List<Emotes>?> GetAllEmotesByIdAsync();

        Task<ResponseModel<Emotes>> SaveEmoteAsync(Emotes emote);

        Task<ResponseModel<Emotes>> DeleteEmoteAsync(int id);
    }
}
