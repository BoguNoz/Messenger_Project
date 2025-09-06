using Models.Entities;
using Raven.Client.Documents.Indexes;

namespace Models.Indexes;

public class Message_ByUser : AbstractIndexCreationTask<Message>
{
    public class Result
    {
        public string SenderId { get; set; }
        public string ReciverId { get; set; }
        public DateTime MessageCreation { get; set; }
    }

    public Message_ByUser()
    {
        Map = messages => from m in messages
            select new
            {
                m.SenderId,
                m.ReciverId,
                m.MessageCreation
            };

        Indexes.Add(x => x.SenderId, FieldIndexing.Default);
        Indexes.Add(x => x.ReciverId, FieldIndexing.Default);
        Indexes.Add(x => x.MessageCreation, FieldIndexing.Default);
    }
}
