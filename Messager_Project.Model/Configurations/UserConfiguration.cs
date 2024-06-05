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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Key Declaration
            builder.HasKey(id => id.User_ID);

            //Assigning a unique key
            builder.Property(id => id.User_ID).ValueGeneratedOnAdd();

            //Propertis of columns
            builder.Property(log => log.Name).HasMaxLength(2000); //For any strin in DB max lenght should be specified
            builder.Property(pn => pn.Surname).HasMaxLength(2000);
            builder.Property(un => un.Username).HasMaxLength(2000);
            builder.Property(up => up.User_Picture).HasMaxLength(20000);
        }
    }
}
