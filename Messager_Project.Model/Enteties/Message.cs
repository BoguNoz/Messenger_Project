using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Model.Enteties
{
    public class Message
    {
        //Message Table Column Framework

        public int Message_ID { get; set; }

        public DateTime Message_Creation { get; set; }

        public string Message_Content { get; set; } = string.Empty;



        //Relations
            //One User -> Meny Message
        public int Sender_ID { get; set; }
        public virtual User Sender { get; set; }

        public int Reciver_ID { get; set; }
        public virtual User Reciver { get; set; }

            //Meny Emotes -> Meny Messages
        public virtual ICollection<MessageEmotes> Emotes { get; set; }

    }
}
