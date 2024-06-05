using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Model.Enteties
{
    public class UserFriends
    {
        //User Friends Table Colums Framework 

        public int Relation_ID { get; set; }

        public DateTime CreationDate { get; set; }

        public int User1_ID { get; set; } //Main
        public virtual User User1 { get; set; }

        public int User2_ID { get; set; }
        public virtual User User2 { get; set; }
    }
}
