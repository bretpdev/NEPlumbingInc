using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace NEPlumbingInc.Services;

public record ResumeUploadResult(
    string BlobName,
    string OriginalFileName,
    string ContentType,
    long SizeBytes);

public interface IResumeStorageService
{
    Task<ResumeUploadResult> UploadResumeAsync(int messageId, IFormFile resumeFile, CancellationToken cancellationToken = default);
    Task<(Stream Content, string ContentType)> OpenResumeReadAsync(string blobName, CancellationToken cancellationToken = default);
}

public class ResumeStorageService(IConfiguration configuration) : IResumeStorageService
{
    private readonly IConfiguration _configuration = configuration;

    private static string? FirstNonEmpty(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value;
        }

        return null;
    }

    private (BlobContainerClient Container, string ContainerName) CreateContainerClient()
    {
        var connectionString = FirstNonEmpty(
            _configuration["ResumeBlobStorage:ConnectionString"],
            _configuration["AzureBlobStorage:ConnectionString"],
            _configuration.GetConnectionString("ResumeBlobStorage"),
            _configuration.GetConnectionString("AzureBlobStorage"));

        var containerName = FirstNonEmpty(
            _configuration["ResumeBlobStorage:ResumeContainer"],
            _configuration["AzureBlobStorage:ResumeContainer"]) ?? "resumes";

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Blob storage connection string is not configured. " +
                "Set it via Azure App Service Configuration using the key 'ResumeBlobStorage__ConnectionString', " +
                "or set 'ResumeBlobStorage:ConnectionString' in appsettings.json / user-secrets for local development. " +
                "(Legacy fallback: 'AzureBlobStorage:ConnectionString'.)");
        }

        return (new BlobContainerClient(connectionString, containerName), containerName);
    }

    public async Task<ResumeUploadResult> UploadResumeAsync(int messageId, IFormFile resumeFile, CancellationToken cancellationToken = default)
    {
        if (resumeFile is null) throw new ArgumentNullException(nameof(resumeFile));

        var originalFileName = Path.GetFileName(resumeFile.FileName);
        var extension = Path.GetExtension(originalFileName);
        var contentType = string.IsNullOrWhiteSpace(resumeFile.ContentType)
            ? "application/octet-stream"
            : resumeFile.ContentType;

        var blobName = $"job-applications/{messageId}/{Guid.NewGuid():N}{extension}";

        var (container, _) = CreateContainerClient();
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var blobClient = container.GetBlobClient(blobName);

        await using var stream = resumeFile.OpenReadStream();
        await blobClient.UploadAsync(
            stream,
            new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
            },
            cancellationToken);

        return new ResumeUploadResult(
            BlobName: blobName,
            OriginalFileName: originalFileName,
            ContentType: contentType,
            SizeBytes: resumeFile.Length);
    }

    public async Task<(Stream Content, string ContentType)> OpenResumeReadAsync(string blobName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(blobName))
            throw new ArgumentException("Blob name is required", nameof(blobName));

        var (container, _) = CreateContainerClient();
        var blobClient = container.GetBlobClient(blobName);

        try
        {
            var result = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
            var contentType = result.Value.Details.ContentType ?? "application/octet-stream";
            return (result.Value.Content, contentType);
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            throw new FileNotFoundException("Resume file not found in blob storage.", blobName, ex);
        }
    }
}
