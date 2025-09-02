using Models.Entities;
using Raven.Client.Documents.Indexes;

namespace Models.Indexes;

public class Messages_CountByUser : AbstractIndexCreationTask<Message, Messages_CountByUser.Result>
{
    public class Result
    {
        public string UserId { get; set; }
        public int Count { get; set; }
    }

    public Messages_CountByUser()
    {
        Map = messages => from m in messages
            from u in new[] { m.SenderId, m.ReciverId }
            select new
            {
                UserId = u,
                Count = 1
            };

        Reduce = results => from result in results
            group result by result.UserId into g
            select new
            {
                UserId = g.Key,
                Count = g.Sum(x => x.Count)
            };
    }
}
