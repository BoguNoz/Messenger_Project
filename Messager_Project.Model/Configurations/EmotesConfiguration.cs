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
    public class EmotesConfiguration : IEntityTypeConfiguration<Emotes>
    {
        public void Configure(EntityTypeBuilder<Emotes> builder)
        {
            //Key Declaration
            builder.HasKey(id => id.Emote_ID);

            //Assigning a unique key
            builder.Property(id => id.Emote_ID).ValueGeneratedOnAdd();

            //Propertis of columns
            builder.Property(en => en.Emote_Name).HasMaxLength(200);
            builder.Property(eu => eu.Emote_Unicode).HasMaxLength(20000);
            builder.Property(edc => edc.Emote_Default_Color).HasMaxLength(200);

            //Relations ---
        }
    }
}
