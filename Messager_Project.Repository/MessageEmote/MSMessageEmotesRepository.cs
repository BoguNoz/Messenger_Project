using Messager_Project.Model;
using Messager_Project.Model.Enteties;
using Messager_Project.Repository.UsersFriends;
using Microsoft.EntityFrameworkCore;
using ResponseModelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Repository.MessageEmote
{
    public class MSMessageEmotesRepository : BaseRepository, IMessageEmotesRepository
    {
        public MSMessageEmotesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<MessageEmotes?> GetMessageEmotesByIdAsync(int id)
        {
            var relation = await DbContext._messageEmotes.SingleOrDefaultAsync(r => r.Relation_ID.Equals(id));

            return relation;
        }

        public async Task<List<MessageEmotes>?> GetAllMessageEmotesAsync()
        {
            var relations = await DbContext._messageEmotes.ToListAsync();

            return relations;
        }

        public async Task<List<MessageEmotes>> GetMessageEmotesByIdAsync(string messageId)
        {
            var relations = await DbContext._messageEmotes.Where(m => m.Message_ID.Equals(messageId)).ToListAsync();

            return relations;
        }

        public async Task<ResponseModel<MessageEmotes>> SaveRelationshipAsync(MessageEmotes relation, Emotes emote, Message message)
        {
            if (relation == null)
                return new ResponseModel<MessageEmotes> { Status = false, Message = "Relation is null", ReferenceObject = null }; 
            if (emote == null)
                return new ResponseModel<MessageEmotes> { Status = false, Message = "Emote is null", ReferenceObject = null };
            if (message == null)
                return new ResponseModel<MessageEmotes> { Status = false, Message = "Message is null", ReferenceObject = null };

            //Checking status
            DbContext.Entry(relation).State = relation.Relation_ID == default(int) ? EntityState.Added : EntityState.Modified;

            try
            {
                emote.Message_Emotes.Add(relation);
                message.Emotes.Add(relation);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ResponseModel<MessageEmotes> { Status = false, Message = $"Error: {ex.Message}", ReferenceObject = relation }; 
            }

            return new ResponseModel<MessageEmotes> { Status = true, Message = "Relation saved sucesfully", ReferenceObject = relation };
        }

        public async Task<ResponseModel<MessageEmotes>> DeleteRelationshipAsync(int id, Emotes emote, Message message)
        {
            var relation = await GetMessageEmotesByIdAsync(id);


            if (relation == null)
                return new ResponseModel<MessageEmotes> { Status = true, Message = "Relation delete sucesfully", ReferenceObject = null };

            emote.Message_Emotes.Remove(relation);
            message.Emotes.Remove(relation);
            DbContext._messageEmotes.Remove(relation);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ResponseModel<MessageEmotes> { Status = false, Message = $"Error: {ex.Message}", ReferenceObject = relation };
            }

            return new ResponseModel<MessageEmotes> { Status = true, Message = "Relation delete sucesfully", ReferenceObject = null };
        }
    }
}
