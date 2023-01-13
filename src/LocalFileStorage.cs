
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class LocalFileStorage : IFileStorage
    {
        private readonly ILogger _logger;

        public LocalFileStorage(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("LocalFileStorage");
        }

        public Task<string[]> FindAllFilesAsync(string path, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Path.IsPathRooted(path))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), path);
                }

                return Task.FromResult(Directory.GetFiles(path));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Task.FromResult<string[]>(null);
            }
        }

        public async Task<bool> SaveFileAsync(string path, byte[] content, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Path.IsPathRooted(path))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), path);
                }

                await System.IO.File.WriteAllBytesAsync(path, content, cancellationToken).ConfigureAwait(false);

                return true;
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
                if (!Path.IsPathRooted(path))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), path);
                }

                return await System.IO.File.ReadAllBytesAsync(path, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public Task DeleteFileAsync(string path, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Path.IsPathRooted(path))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), path);
                }

                System.IO.File.Delete(path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}
