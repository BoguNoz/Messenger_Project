using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.DTO.Emotes
{
    public class EmotesDto
    {
        [Required]
        [MaxLength(200)]
        public string Emote_Name { get; set; }

        [Required]
        [MaxLength(20000)]
        public string Emote_Unicode { get; set;}

        [Required]
        [MaxLength(200)]
        public string Emote_Default_Color { get; set; }

    }
}
