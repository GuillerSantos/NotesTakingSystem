using System.Security.Claims;

namespace NTS.Server.Utilities
{
    public class UserClaimUtil
    {
        #region Public Methods

        public static bool TryGetUserId(ClaimsPrincipal user, out Guid userId)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out userId);
        }

        #endregion Public Methods
    }
}