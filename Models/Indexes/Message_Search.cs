using Models.Entities;
using Raven.Client.Documents.Indexes;

namespace Models.Indexes;

public class Message_Search : AbstractIndexCreationTask<Message>
{
    public Message_Search()
    {
        Map = messages => from m in messages
            select new
            {
                m.Content,
                m.SenderId,
                m.ReciverId
            };

        Indexes.Add(x => x.Content, FieldIndexing.Search);
    }
}