namespace Common.DTO.Message;

public class MessageRequestDto
{
    public DateTime MessageCreation { get; set; }

    public string Content { get; set; } = string.Empty;
    
    public string SenderId { get; set; }

    public string ReciverId { get; set; }
    
    public virtual List<string> EmoteIds { get; set; }
}