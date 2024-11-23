namespace BabySitting.Api.Contracts;
public class EmailConfirmationRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
