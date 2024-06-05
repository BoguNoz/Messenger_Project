    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Model.Enteties
{
    public class Emotes
    {
        //Emote Table Column Framework

        public int Emote_ID { get; set; }

        public string Emote_Name { get; set; } = string.Empty;

        public string Emote_Unicode { get; set; } = string.Empty;

        public string Emote_Default_Color { get; set; } = string.Empty;


        //Relations
            //Meny Emotes -> Meny Messages
        public virtual ICollection<MessageEmotes> Message_Emotes { get; set; }
    }
}
