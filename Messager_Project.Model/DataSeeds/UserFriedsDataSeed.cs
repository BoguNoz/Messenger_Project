using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Model.DataSeeds
{
    public class UserFriedsDataSeed
    {
        //Creating a starting seed
        public static void Initialize(IServiceProvider service)
        {
            using var context = new AppDbContext(service.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (context._usersFriends.Any())
                return;

            context._usersFriends.Add(new Enteties.UserFriends()
            {
                CreationDate = DateTime.Now,
                User1_ID = 1,
                User2_ID = 2,
            });

            context.SaveChanges();
        }
    }
}
