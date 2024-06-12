using Messager_Project.Model;
using Messager_Project.Model.Enteties;
using Microsoft.EntityFrameworkCore;
using ResponseModelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Repository.Messages
{
    public class MSMessagesRepository : BaseRepository, IEmotesRepository
    {
        public MSMessagesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Message?> GetMessageByIdAsync(int id)
        {
            var message = await DbContext._messages.Include(m => m.Sender).SingleOrDefaultAsync(m => m.Message_ID == id);

            return message;
        }

        public async Task<List<Message>?> GetReciverByIdAsync(int ReciverId)
        {
            var messages = await DbContext._messages.OrderBy(d => d.Message_Creation).Where(m => m.Reciver_ID == ReciverId).ToListAsync();

            return messages;
        }

        public async Task<List<Message>?> GetSenderByIdAsync(int SenderId)
        {
            var messages = await DbContext._messages.OrderBy(d => d.Message_Creation).Where(m => m.Sender_ID == SenderId).ToListAsync();

            return messages;
        }

        public async Task<List<Message>?> GetAllMessageSendToByIdAsync(int SenderId, int ReciverId)
        {
            var senderMessages = await GetSenderByIdAsync(SenderId);
            if (senderMessages == null)
                return (List<Message>?)Enumerable.Empty<Message>();

            var mesages = senderMessages.OrderBy(d => d.Message_Creation).Where(id => id.Reciver_ID == ReciverId).ToList();

            return mesages;

        }

        public async Task<List<Message>?> GetAllMessageRecivedFromByIdAsync(int ReciverId, int SenderId)
        {
            var reciverMessages = await GetReciverByIdAsync(ReciverId);
            if (reciverMessages == null)
                return (List<Message>?)Enumerable.Empty<Message>();

            var mesages = reciverMessages.OrderBy(d => d.Message_Creation).Where(id => id.Sender_ID == SenderId).ToList();

            return mesages;

        }

        public async Task<List<Message>?> GetAllMessagesAsync()
        {
            var messages = await DbContext._messages.OrderBy(d => d.Message_Creation).ToListAsync();

            return messages;
        }

        public async Task<ResponseModel<Message>> SaveRelationAsync(Message relation)
        {
            if (relation == null)
                return new ResponseModel<Message> { Status = false, Message = "Relation is null", ReferenceObject = relation };

            //Checking status
            DbContext.Entry(relation).State = relation.Message_ID == default(int) ? EntityState.Added : EntityState.Modified;
            //var reciver = await DbContext._users.SingleOrDefaultAsync(r => r.User_ID == relation.Reciver_ID);
            //var sendrer = await DbContext._users.SingleOrDefaultAsync(s => s.User_ID == relation.Sender_ID);

            try
            {

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ResponseModel<Message> { Status = false, Message = $"Error: {ex.Message}", ReferenceObject = relation };
            }

            return new ResponseModel<Message> { Status = true, Message = "Message saved successfully", ReferenceObject = relation };
        }

        public async Task<ResponseModel<Message>> DeleteRelationAsync(int id)
        {
            var relation = await GetMessageByIdAsync(id);


            if (relation == null)
                return new ResponseModel<Message> { Status = true, Message = "Message deleted successfully", ReferenceObject = relation };

            DbContext._messages.Remove(relation);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ResponseModel<Message> { Status = false, Message = $"Error: {ex.Message}", ReferenceObject = relation }; 
            }

            return new ResponseModel<Message> { Status = true, Message = "Message deleted successfully", ReferenceObject = relation };
        }
    }
}
