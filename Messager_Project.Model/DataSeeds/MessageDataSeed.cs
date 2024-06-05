using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Model.DataSeeds
{
    public class MessageDataSeed
    {
        //Creating a starting seed
        public static void Initialize(IServiceProvider service)
        {
            using var context = new AppDbContext(service.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (context._messages.Any())
                return;

            context._messages.Add(new Enteties.Message()
            {
                Message_Creation = DateTime.Now,
                Message_Content = "Cześć",
                Sender_ID = 1,
                Reciver_ID = 2,
            });

            context._messages.Add(new Enteties.Message()
            {
                Message_Creation = DateTime.Now,
                Message_Content = "Hello",
                Sender_ID = 2,
                Reciver_ID = 1,
            });

            context.SaveChanges();
        }
    }
}
