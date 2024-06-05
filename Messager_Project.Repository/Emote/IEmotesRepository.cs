using Messager_Project.Model.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Repository.Emote
{
    internal interface IEmotesRepository
    {
        Task<Emotes?> GetEmotesByIdAsync(int id);

        Task<List<Emotes>?> GetAllEmotesByIdAsync();

        Task<bool> SaveEmoteAsync(Emotes emote);

        Task<bool> DeleteEmoteAsync(int id);
    }
}
