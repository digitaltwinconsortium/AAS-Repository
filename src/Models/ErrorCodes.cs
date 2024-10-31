namespace UaRestGateway.Server.Model
{
    public static class ErrorCodes
    {
        public const string UnexpectedError = "An unexpected error occurred.";

        public const string NotLoggedIn = "No user is logged in.";

        public const string UserNotRegistered = "The user has not registered with the application.";

        public static string ServerNotRunning = "The server is not running.";
    }
}
