
namespace AdminShell.Interfaces
{
    public interface IUserService
    {
        bool ValidateCredentials(string username, string password);
    }
}

