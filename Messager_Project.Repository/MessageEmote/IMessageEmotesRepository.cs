using Messager_Project.Model.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Repository.MessageEmote
{
    public interface IMessageEmotesRepository
    {
        Task<MessageEmotes?> GetMessageEmotesByIdAsync(int id);

        Task<List<MessageEmotes>?> GetAllMessageEmotesAsync();

        //Gets all emotes that are connected to given message
        Task<List<MessageEmotes>> GetMessageEmotesByIdAsync(string MessageId);

        Task<bool> SaveRelationshipAsync(MessageEmotes relation, Emotes emote, Message message);

        Task<bool> DeleteRelationshipAsync(int id, Emotes emote, Message message);
    }
}
