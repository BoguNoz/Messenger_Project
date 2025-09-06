using Microsoft.AspNetCore.Http;
using Models.Entities;

namespace Services.Storage;

public interface IBlobStorageService
{
    Task<List<BlobObject>> GetAllBlobsAsync();
    Task<string> GetBlobUrl(string blobName);
    Task<BlobObject> UploadAsync(IFormFile fromFile);
    Task UploadMetadataAsync(BlobObject blob, Dictionary<string, string> metadata);
}
