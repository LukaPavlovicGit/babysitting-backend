namespace BabySitting.Api.Exceptions;

public class NotFoundException : Exception
{
    public static readonly int StatusCode = StatusCodes.Status404NotFound;
    public NotFoundException(string message) : base(message) { }
}