namespace Common.DTO.User;

public class UserResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<string> PicturesLinks { get; set; }
}