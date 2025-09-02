using Models.Entities;
using Raven.Client.Documents.Indexes;

namespace Models.Indexes;

public class Message_ByUser : AbstractIndexCreationTask<Message>
{
    public class Result
    {
        public string UserId { get; set; }
        public DateTime MessageCreation { get; set; }
    }

    public Message_ByUser()
    {
        Map = messages => from m in messages
            from userId in new[] { m.SenderId, m.ReciverId }
            select new
            {
                UserId = userId,
                MessageCreation = m.MessageCreation
            };
        Indexes.Add(x => x.MessageCreation, FieldIndexing.Default);
        Indexes.Add(x => x.Id, FieldIndexing.Default);
    }
}