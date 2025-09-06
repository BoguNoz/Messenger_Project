using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Models.Entities;

namespace Services.Storage;

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly string _connectionString;
    private readonly string _containerName;

    public AzureBlobStorageService(IConfiguration configuration)
    {
        _connectionString = configuration["AzureBlobStorage:ConnectionString"];
        _containerName = configuration["AzureBlobStorage:ContainerName"];
    }

    private BlobContainerClient GetBlobContainerClient()
    {
        var client = new BlobContainerClient(_connectionString, _containerName);
        client.CreateIfNotExists(PublicAccessType.Blob);
        return client;
    }

    public async Task<List<BlobObject>> GetAllBlobsAsync()
    {
        var containerClient = GetBlobContainerClient();
        var blobs = new List<BlobObject>();

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            var blobClient = containerClient.GetBlobClient(blobItem.Name);
            var blobProperties = await blobClient.GetPropertiesAsync();
            blobProperties.Value.Metadata.TryGetValue("Caption", out var caption);

            blobs.Add(new BlobObject
            {
                Name = blobItem.Name,
                ImageUri = blobClient.Uri.ToString(), 
            });
        }

        return blobs;
    }

    public async Task<BlobObject> UploadAsync(IFormFile fromFile)
    {
        var containerClient = GetBlobContainerClient();
        var blobClient = containerClient.GetBlobClient(fromFile.FileName);

        await using var stream = fromFile.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        return new BlobObject
        {
            Name = fromFile.FileName,
            ImageUri = blobClient.Uri.ToString(),
        };
    }

    public async Task UploadMetadataAsync(BlobObject blob, Dictionary<string, string> metadata)
    {
        var containerClient = GetBlobContainerClient();
        var blobClient = containerClient.GetBlobClient(blob.Name);

        if (!await blobClient.ExistsAsync())
            throw new Exception($"Blob {blob.Name} does not exist.");

        await blobClient.SetMetadataAsync(metadata);
    }

    public async Task<string> GetBlobUrl(string blobName)
    {
        var containerClient = GetBlobContainerClient();
        var blobClient = containerClient.GetBlobClient(blobName);

        if (await blobClient.ExistsAsync())
        {
            return blobClient.Uri.ToString();
        }

        return string.Empty; 
    }
}
