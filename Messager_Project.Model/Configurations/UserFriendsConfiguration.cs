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
    public class UserFriendsConfiguration : IEntityTypeConfiguration<UserFriends>
    {
        public void Configure(EntityTypeBuilder<UserFriends> builder)
        {
            //Key Declaration
            builder.HasKey(id => id.Relation_ID);
            builder.Property(id => id.Relation_ID).ValueGeneratedOnAdd();

            //Relations
            builder.HasOne(u1 => u1.User1).WithMany(u2 => u2.User_Friends).HasForeignKey(id => id.User1_ID);
            builder.HasOne(u2 => u2.User2).WithMany(u1 => u1.Frinds_With_User).HasForeignKey(id => id.User2_ID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
