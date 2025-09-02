namespace Common.DTO.User;

public class UserRequestDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } = string.Empty;
    public List<string> PicturesLinks { get; set; } 
    public List<string>? Friends { get; set; }

}