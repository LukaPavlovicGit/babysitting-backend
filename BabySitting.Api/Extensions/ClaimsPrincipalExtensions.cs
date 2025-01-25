using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
        => principal.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? string.Empty;

    public static string GetEmail(this ClaimsPrincipal principal)
        => principal.FindFirstValue(JwtRegisteredClaimNames.Email) ?? string.Empty;

    public static string GetRole(this ClaimsPrincipal principal)
        => principal.FindFirstValue("Role") ?? string.Empty;
}