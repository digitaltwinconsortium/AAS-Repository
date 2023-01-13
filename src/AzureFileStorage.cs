
using System.Threading.Tasks;
using System.Threading;

namespace AdminShell
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class AzureFileStorage : IFileStorage
    {
        private readonly ILogger _logger;

        public AzureFileStorage(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("AzureFileStorage");
        }

        public async Task<string[]> FindAllFilesAsync(string path, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BlobStorageConnectionString")))
                {
                    // open blob storage
                    BlobContainerClient container = new BlobContainerClient(Environment.GetEnvironmentVariable("BlobStorageConnectionString"), "aasrepo");
                    await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

                    var resultSegment = container.GetBlobsAsync();

                    List<string> files = new List<string>();
                    await foreach (BlobItem blobItem in resultSegment.ConfigureAwait(false))
                    {
                        files.Add(blobItem.Name);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> SaveFileAsync(string path, byte[] content, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BlobStorageConnectionString")))
                {
                    // open blob storage
                    BlobContainerClient container = new BlobContainerClient(Environment.GetEnvironmentVariable("BlobStorageConnectionString"), "aasrepo");
                    await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

                    // Get a reference to the blob
                    BlobClient blob = container.GetBlobClient(path);

                    // Open the file and upload its data
                    using (MemoryStream file = new MemoryStream(content))
                    {
                        await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
                        await blob.UploadAsync(file, cancellationToken).ConfigureAwait(false);

                        // Verify uploaded
                        BlobProperties properties = await blob.GetPropertiesAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
                        if (file.Length != properties.ContentLength)
                        {
                            throw new Exception("Could not verify upload!");
                        }

                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return false;
            }
        }

        public async Task<byte[]> LoadFileAsync(string path, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BlobStorageConnectionString")))
                {
                    // open blob storage
                    BlobContainerClient container = new BlobContainerClient(Environment.GetEnvironmentVariable("BlobStorageConnectionString"), "aasrepo");
                    await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

                    var resultSegment = container.GetBlobsAsync();
                    await foreach (BlobItem blobItem in resultSegment.ConfigureAwait(false))
                    {
                        if (blobItem.Name.Equals(path))
                        {
                            // Get a reference to the blob
                            BlobClient blob = container.GetBlobClient(blobItem.Name);

                            // Download the blob's contents and save it in memory
                            BlobDownloadInfo download = await blob.DownloadAsync(cancellationToken).ConfigureAwait(false);
                            using (MemoryStream file = new MemoryStream())
                            {
                                await download.Content.CopyToAsync(file, cancellationToken).ConfigureAwait(false);

                                // Verify download
                                BlobProperties properties = await blob.GetPropertiesAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
                                if (file.Length != properties.ContentLength)
                                {
                                    throw new Exception("Could not verify upload!");
                                }

                                return file.ToArray();
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return null;
            }
        }

        public async Task DeleteFileAsync(string path, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BlobStorageConnectionString")))
                {
                    // open blob storage
                    BlobContainerClient container = new BlobContainerClient(Environment.GetEnvironmentVariable("BlobStorageConnectionString"), "aasrepo");

                    var response = await container.DeleteBlobAsync(path, cancellationToken: cancellationToken).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
