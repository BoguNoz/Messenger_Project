using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PasswordEncryptionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Messager_Project.Model.DataSeeds
{
    public class UserDataSeed
    {
        //Creating a starting seed
        public static void Initialize(IServiceProvider service)
        {
            using var context = new AppDbContext(service.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (context._users.Any())
                return;

            Random rand = new Random();
            PasswordEncryption password = new PasswordEncryption();
            var salt = password.GetSalt();

            context._users.Add(new Enteties.User()
            {
                Name = "Jan",
                Surname = "Kowalski",
                Username = "JK",
                User_Picture = "",
                PasswordHash = password.GenerateSaltedHash(Encoding.UTF8.GetBytes("JK123"), salt),
                PasswordSalt = salt

            });

            context._users.Add(new Enteties.User()
            {
                Name = "John",
                Surname = "Smith",
                Username = "JS",
                User_Picture = "",
                PasswordHash = password.GenerateSaltedHash(Encoding.UTF8.GetBytes("JS321"), salt),
                PasswordSalt = salt

            });

            context.SaveChanges();
        }

    }
}
