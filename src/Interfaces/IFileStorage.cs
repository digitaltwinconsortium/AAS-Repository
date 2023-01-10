
namespace AdminShell
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IFileStorage
    {
        Task<string[]> FindAllFilesAsync(string path, CancellationToken cancellationToken = default);

        Task<bool> SaveFileAsync(string path, byte[] content, CancellationToken cancellationToken = default);

        Task<byte[]> LoadFileAsync(string path, CancellationToken cancellationToken = default);

        Task DeleteFileAsync(string path, CancellationToken cancellationToken = default);
    }
}
