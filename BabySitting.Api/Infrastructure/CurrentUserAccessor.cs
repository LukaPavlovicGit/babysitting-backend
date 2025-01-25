public class CurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) 
        => _httpContextAccessor = httpContextAccessor;

    public CurrentUser User => new(
        _httpContextAccessor.HttpContext?.User?.GetUserId() ?? string.Empty,
        _httpContextAccessor.HttpContext?.User?.GetEmail() ?? string.Empty,
        _httpContextAccessor.HttpContext?.User?.GetRole() ?? string.Empty
    );
}