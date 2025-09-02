using Microsoft.Extensions.Configuration;
using Models.Indexes;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

public class DbContext : IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IDocumentStore _documentStore;

    public IDocumentStore DocumentStore => _documentStore;
    
    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration;

        var url = _configuration["RavenSettings:Url"];
        var databaseName = _configuration["RavenSettings:DatabaseName"];

        _documentStore = new DocumentStore
        {
            Urls = new[] { url },
            Database = databaseName
        };

        _documentStore.Initialize();
    }

    public Task InitializeIndexesAsync()
    {
        new Message_Search().ExecuteAsync(_documentStore);
        new Message_ByUser().ExecuteAsync(_documentStore);
        new Emote_ByMessageIndex().ExecuteAsync(_documentStore);
        return Task.CompletedTask;
    }

    public async Task<IAsyncDocumentSession> OpenAsyncSession()
    {
        return _documentStore.OpenAsyncSession();
    }

    public void Dispose()
    {
        _documentStore?.Dispose();
    }
}
