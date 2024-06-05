using Messager_Project.Model;
using Messager_Project.Model.Enteties;
using Messager_Project.Repository.Messages;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> SaveEmoteAsync(Emotes emote)
        {
            if(emote == null)
                return false;

            if(DbContext._emotes.Any(e => e.Emote_Name.Equals(emote.Emote_Name, StringComparison.OrdinalIgnoreCase)))
                return false;

            //Checking status
            DbContext.Entry(emote).State = emote.Emote_ID == default(int) ? EntityState.Added : EntityState.Modified;

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

        public async Task<bool> DeleteEmoteAsync(int id)
        {
            var emote = await GetEmotesByIdAsync(id);

            if (emote == null)
                return true;

            DbContext._emotes.Remove(emote); //Hard Removal

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
