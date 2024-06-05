using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Model.Enteties
{
    public class User
    {
        //User Table Colums Framework 

        public int User_ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string? User_Picture { get; set; }

        //Password and sead 
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];

        //Relations
            //Meny User -> Many Friends
        public virtual ICollection<UserFriends> User_Friends { get; set; }
        public virtual ICollection<UserFriends> Frinds_With_User { get; set; }

            //One User -> Meny Message
        public virtual ICollection<Message> MessagesSent { get; set; }
        public virtual ICollection<Message> MessagesReceived { get; set; }

    }
}
