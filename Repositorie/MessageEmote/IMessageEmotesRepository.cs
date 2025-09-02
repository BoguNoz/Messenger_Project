using Messager_Project.Model.Enteties;
using ResponseModelService;
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

        Task<ResponseModel<MessageEmotes>> SaveRelationshipAsync(MessageEmotes relation, Emotes emote, Message message);

        Task<ResponseModel<MessageEmotes>> DeleteRelationshipAsync(int id, Emotes emote, Message message);
    }
}
