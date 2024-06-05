using Messager_Project.Model.Enteties;
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

        Task<List<Message>?> GetAllMessagesAsync();

        Task<bool> SaveRelationAsync(Message relation);

        Task<bool> DeleteRelationAsync(int id); //#Zmienić boola na obiekt zwracający stan wykonania
    }
}
