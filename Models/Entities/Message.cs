namespace Models.Entities;

public class Message
{
    public string Id { get; set; }

    public DateTime MessageCreation { get; set; }

    public string Content { get; set; } = string.Empty;
    
    public string AttachmentUrl { get; set; } = string.Empty;
    
    public string SenderId { get; set; }

    public string ReciverId { get; set; }
    
    public virtual ICollection<string> Emotes { get; set; }

}

