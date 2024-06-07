using Messager_Project.Model;
using Messager_Project.Model.Enteties;
using Messager_Project.Repository.Messages;
using Microsoft.EntityFrameworkCore;
using ResponseModelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Repository.Emote
{
    public class MSEmotesRepository : BaseRepository, IEmotesRepository
    {
    
        public MSEmotesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Emotes?> GetEmotesByIdAsync(int id)
        {
            var emote = await DbContext._emotes.SingleOrDefaultAsync(e => e.Emote_ID.Equals(id));

            return emote;
        }

        public async Task<List<Emotes>?> GetAllEmotesByIdAsync()
        {
            var emotes = await DbContext._emotes.ToListAsync();

            return emotes;
        }

        public async Task<ResponseModel<Emotes>> SaveEmoteAsync(Emotes emote)
        {
            if(emote == null)
                return new ResponseModel<Emotes> { Status = false, Message = "Emote is null", ReferenceObject = null };

            if(DbContext._emotes.Any(e => e.Emote_Name.Equals(emote.Emote_Name)))
                return new ResponseModel<Emotes> { Status = false, Message = "Emote name arledy exist in data base", ReferenceObject = emote }; 

            //Checking status
            DbContext.Entry(emote).State = emote.Emote_ID == default(int) ? EntityState.Added : EntityState.Modified;

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ResponseModel<Emotes> { Status = false, Message = $"Error: {ex.Message}" , ReferenceObject = emote };
            }

            return new ResponseModel<Emotes> { Status = true, Message = "Emote saved successfully", ReferenceObject = emote };
        }

        public async Task<ResponseModel<Emotes>> DeleteEmoteAsync(int id)
        {
            var emote = await GetEmotesByIdAsync(id);

            if (emote == null)
                return new ResponseModel<Emotes> { Status = true, Message = "Emote delete successfully", ReferenceObject = null };

            DbContext._emotes.Remove(emote); //Hard Removal

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ResponseModel<Emotes> { Status = false, Message = $"Error: {ex.Message}", ReferenceObject = emote };
            }

            return new ResponseModel<Emotes> { Status = true, Message = "Emote delete successfully", ReferenceObject = emote };
        }

    }
}
