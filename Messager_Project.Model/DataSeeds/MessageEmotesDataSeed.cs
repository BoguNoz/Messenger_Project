using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Model.DataSeeds
{
    public class MessageEmotesDataSeed
    {
        //Creating a starting seed
        public static void Initialize(IServiceProvider service)
        {
            using var context = new AppDbContext(service.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (context._messageEmotes.Any())
                return;

            context._messageEmotes.Add(new Enteties.MessageEmotes()
            {
                Message_ID = 1,
                Emote_ID = 1,
            });

            context.SaveChanges();
        }
    }
}
