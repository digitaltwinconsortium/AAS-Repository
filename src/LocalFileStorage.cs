
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Azure storage class
    /// </summary>
    public class LocalFileStorage : IFileStorage
    {
        private readonly ILogger _logger;
        private readonly string _rootDir;

        /// <summary>
        /// Default constructor
        /// </summary>
        public LocalFileStorage(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("LocalFileStorage");
            _rootDir = Path.Combine(Path.GetTempPath(), "CloudLib");
        }

        /// <summary>
        /// Find a file based on a unique name
        /// </summary>
        public Task<string> FindFileAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                if (System.IO.File.Exists(Path.Combine(_rootDir, name)))
                {
                    return Task.FromResult(name);
                }
                else
                {
                    return Task.FromResult<string>(null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Task.FromResult<string>(null);
            }
        }

        /// <summary>
        /// Upload a file to a blob and return the filename for storage in the index db
        /// </summary>
        public async Task<string> UploadFileAsync(string name, string content, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Directory.Exists(_rootDir))
                {
                    Directory.CreateDirectory(_rootDir);
                }
                await System.IO.File.WriteAllTextAsync(Path.Combine(_rootDir, name), content).ConfigureAwait(false);
                return name;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Download a blob to a file.
        /// </summary>
        public async Task<string> DownloadFileAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                return await System.IO.File.ReadAllTextAsync(Path.Combine(_rootDir, name)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
        public Task DeleteFileAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                System.IO.File.Delete(Path.Combine(_rootDir, name));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            return Task.CompletedTask;
        }
    }
}
