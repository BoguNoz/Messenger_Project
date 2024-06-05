using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Model.DataSeeds
{
    public class EmotesDataSeed
    {
        //Creating a starting seed
        public static void Initialize(IServiceProvider service)
        {
            using var context = new AppDbContext(service.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (context._emotes.Any())
                return;

            context._emotes.Add(new Enteties.Emotes()
            {
                Emote_Name = "Heart_Solid",
                Emote_Unicode = "f004",
                Emote_Default_Color = "#FF5C5C"

            });

            context._emotes.Add(new Enteties.Emotes()
            {
                Emote_Name = "Thumbs-up_Solid",
                Emote_Unicode = "f164",
                Emote_Default_Color = "#FFBF00"

            });

            context._emotes.Add(new Enteties.Emotes()
            {
                Emote_Name = "Face-smile_Solid",
                Emote_Unicode = "f118",
                Emote_Default_Color = "#FFBF00"

            });

            context._emotes.Add(new Enteties.Emotes()
            {
                Emote_Name = "Face-sad-cry_Solid",
                Emote_Unicode = "f5b3",
                Emote_Default_Color = "#FFBF00"

            });

            context._emotes.Add(new Enteties.Emotes()
            {
                Emote_Name = "Face-angry_Solid",
                Emote_Unicode = "f556",
                Emote_Default_Color = "#FFBF00"

            });

            context._emotes.Add(new Enteties.Emotes()
            {
                Emote_Name = "Face-surprise_Solid",
                Emote_Unicode = "f5c2",
                Emote_Default_Color = "#FFBF00"

            });

            context.SaveChanges();
        }

       
    }
}
