using Common;
using Common.DTO.User;
using Models.Entities;

namespace Services.Adapters;

public class UserAdapter
{
    public static Task<User> ConvertRequestDtoToModel(UserRequestDto userDto)
    {
        HashFunction hashFunction = new HashFunction();
        var (hash, salt) = hashFunction.Hash(userDto.Password);

        return Task.FromResult(new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            PasswordHash = hash,
            PasswordSalt = salt,
            PicturesLinks = userDto.PicturesLinks,
            Friends = userDto.Friends,
        });
    }

    public static Task<UserResponseDto> ConvertModelToResponseDto(User user)
    {
        return Task.FromResult(new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PicturesLinks = user.PicturesLinks.ToList(),
        });
    }
}