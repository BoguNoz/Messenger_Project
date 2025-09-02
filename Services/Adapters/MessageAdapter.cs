using Common.DTO.Message;
using Models.Entities;

namespace Services.Adapters;

public class MessageAdapter
{
    public static Task<Message> ConvertRequestDtoToModel(MessageRequestDto messageDto)
    {
        return Task.FromResult(new Message
        {
            MessageCreation = messageDto.MessageCreation,
            Content = messageDto.Content,
            SenderId = messageDto.SenderId,
            ReciverId = messageDto.ReciverId,
            Emotes = messageDto.EmoteIds,
   
        });
    }

    public static Task<MessageResponseDto> ConvertModelToResponseDto(Message message)
    {
        return Task.FromResult(new MessageResponseDto
        {
            MessageCreation = message.MessageCreation,
            Content = message.Content,
            SenderId = message.SenderId,
            ReciverId = message.ReciverId,
            EmoteIds = message.Emotes.ToList()
        });
    }
}