using Messager_Project.Model;
using Messager_Project.Model.Enteties;
using Microsoft.EntityFrameworkCore;
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
            var messages = await DbContext._messages.Where(m => m.Reciver_ID == ReciverId).ToListAsync();

            return messages;
        }

        public async Task<List<Message>?> GetSenderByIdAsync(int SenderId)
        {
            var messages = await DbContext._messages.Where(m => m.Sender_ID == SenderId).ToListAsync();

            return messages;
        }

        public async Task<List<Message>?> GetAllMessagesAsync()
        {
            var messages = await DbContext._messages.ToListAsync();

            return messages;
        }

        public async Task<bool> SaveRelationAsync(Message relation)
        {
            if (relation == null)
                return false;

            //Checking status
            DbContext.Entry(relation).State = relation.Message_ID == default(int) ? EntityState.Added : EntityState.Modified;

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false; //#Zmień na obiekt
            }

            return true;
        }

        public async Task<bool> DeleteRelationAsync(int id)
        {
            var relation = await GetMessageByIdAsync(id);


            if (relation == null)
                return true;

            DbContext._messages.Remove(relation);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
