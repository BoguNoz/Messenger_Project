using Messager_Project.Model.Enteties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Model.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Message> builder)
        {
            //Key Declaration
            builder.HasKey(id => id.Message_ID);

            //Assigning a unique key
            builder.Property(id => id.Message_ID).ValueGeneratedOnAdd();

            //Propertis of columns
            builder.Property(mn => mn.Message_Content).HasMaxLength(20000);

            //Relations
            builder.HasOne(m => m.Sender)
           .WithMany(u => u.MessagesSent)
           .HasForeignKey(m => m.Sender_ID).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Reciver)
                .WithMany(u => u.MessagesReceived)
                .HasForeignKey(m => m.Reciver_ID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
