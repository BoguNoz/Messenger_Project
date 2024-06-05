using Messager_Project.Model.Enteties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Model.Configurations
{
    public class MessageEmotesConfiguration : IEntityTypeConfiguration<MessageEmotes>
    {
        public void Configure(EntityTypeBuilder<MessageEmotes> builder)
        {
            //Key Declaration
            builder.HasKey(id => id.Relation_ID);
            builder.Property(id => id.Relation_ID).ValueGeneratedOnAdd();

            //Relations
            builder.HasOne(m => m.Message).WithMany(me => me.Emotes).HasForeignKey(id => id.Message_ID);
            builder.HasOne(e => e.Emote).WithMany(em => em.Message_Emotes).HasForeignKey(id => id.Emote_ID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
