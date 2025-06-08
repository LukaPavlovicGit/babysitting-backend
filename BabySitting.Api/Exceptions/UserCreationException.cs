namespace BabySitting.Api.Exceptions;

public class UserCreationException : Exception
{
    public static readonly int StatusCode = StatusCodes.Status409Conflict;
    public UserCreationException(string message) : base(message) { }
}