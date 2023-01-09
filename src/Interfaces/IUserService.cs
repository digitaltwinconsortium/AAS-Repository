
namespace AdminShell
{
    public interface IUserService
    {
        bool ValidateCredentials(string username, string password);
    }
}

