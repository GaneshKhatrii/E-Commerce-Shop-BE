using System.Security.Claims;

namespace ECommerce.API.Helpers
{
    public static class CurrentUserHelper
    {
        public static Guid GetUserId(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userId!);
        }
    }
}
