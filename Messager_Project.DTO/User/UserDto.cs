using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.DTO.User
{
    public class UserInputDto
    {
        [Required]
        [MaxLength(2000)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Surname { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Username { get; set; }

        public string? User_Picture { get; set; }
    }
}
