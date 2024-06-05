using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Model.Enteties
{
    public class MessageEmotes
    {
        //Message Emotes Table Colums Framework 

        public int Relation_ID { get; set; }

        public int Message_ID { get; set;}
        public virtual Message Message { get; set; }

        public int Emote_ID { get; set; }
        public virtual Emotes Emote { get; set; }
    }
}
