namespace BabySitting.Api.Exceptions;

public class ValidationException : Exception
{   
    public static readonly int StatusCode = StatusCodes.Status400BadRequest;
    public ValidationException(string message) : base(message) { }
} 