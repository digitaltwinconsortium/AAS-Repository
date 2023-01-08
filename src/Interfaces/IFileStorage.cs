
namespace AdminShell
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IFileStorage
    {
        Task<string> FindFileAsync(string name, CancellationToken cancellationToken = default);

        Task<string> UploadFileAsync(string name, string content, CancellationToken cancellationToken = default);

        Task<string> DownloadFileAsync(string name, CancellationToken cancellationToken = default);
  
        Task DeleteFileAsync(string name, CancellationToken cancellationToken = default);
    }
}
