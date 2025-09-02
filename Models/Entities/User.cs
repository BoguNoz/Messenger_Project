using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Entities;

public class User
{
    public string Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public ICollection<string> PicturesLinks { get; set; }

    public string PasswordHash { get; set; } = string.Empty;
    
    public string PasswordSalt { get; set; } = string.Empty;
    
    public ICollection<string> Friends { get; set; }
    
    public ICollection<string> Messages { get; set; }
    
}

