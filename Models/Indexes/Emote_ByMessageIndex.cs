using Models.Entities;
using Raven.Client.Documents.Indexes;

namespace Models.Indexes;

public class Emote_ByMessageIndex : AbstractIndexCreationTask<Message>
{
    public class Result
    {
        public string MessageId { get; set; }
        public string EmoteId   { get; set; }
    }

    public Emote_ByMessageIndex()
    {
        Map = messages => from m in messages
            from eId in m.Emotes
            select new
            {
                MessageId = m.Id,
                EmoteId   = eId
            };

        Indexes.Add(x => x.Id, FieldIndexing.Default);
        Indexes.Add(x => x.Id,   FieldIndexing.Default);
    }
}