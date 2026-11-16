using System.Security.Claims;
using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetRequiredUserId(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        return int.TryParse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) && userId > 0
            ? userId
            : throw new AuthenticatedUserClaimsInvalidException();
    }
}
