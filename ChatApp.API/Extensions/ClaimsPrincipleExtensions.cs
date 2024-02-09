using System.Security.Claims;

namespace ChatApp.API.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
