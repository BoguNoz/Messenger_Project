using Messager_Project.Model.Enteties;
using ResponseModelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Repository.Messages
{
    public interface IEmotesRepository
    {
        Task<Message?> GetMessageByIdAsync(int id);

        Task<List<Message>?> GetReciverByIdAsync(int ReciverId);

        Task<List<Message>?> GetSenderByIdAsync(int SenderId);

        Task<List<Message>?> GetAllMessageSendToByIdAsync(int SenderId, int ReciverId);

        Task<List<Message>?> GetAllMessageRecivedFromByIdAsync(int ReciverId, int SenderId);

        Task<List<Message>?> GetAllMessagesAsync();

        Task<ResponseModel<Message>> SaveRelationAsync(Message relation);

        Task<ResponseModel<Message>> DeleteRelationAsync(int id); 
    }
}
