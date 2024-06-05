using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.DTO.Message
{
    public class MessageDto
    {
        [Required]
        [MaxLength(20000)]
        public string Message_Content { get; set; }

        [MaxLength(20000)]
        public string Messsge_Emote { get; set; }

    }
}
