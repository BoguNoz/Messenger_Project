using Messager_Project.Model.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messager_Project.Model.Enteties;

namespace Messager_Project.Model
{
    //Creating connections between Application and Database
    public class AppDbContext : DbContext
    {
        //Tables definition
        public DbSet<User> _users { get; set; }

        public DbSet<UserFriends> _usersFriends { get; set; }

        public DbSet<Message> _messages { get; set; }

        public DbSet<Emotes> _emotes { get; set; }

        public DbSet<MessageEmotes> _messageEmotes { get; set; }


        //Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Adding Configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserFriendsConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new EmotesConfiguration());
            modelBuilder.ApplyConfiguration(new MessageEmotesConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
