namespace BabySitting.Api.Exceptions;

public class UserLoginException : Exception
{
    public static readonly int StatusCode = StatusCodes.Status400BadRequest;
    public UserLoginException(string message) : base(message) { }
}