using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PickAndEat.Extensions {
  public static class ClaimsPrincipalExtensions {
    public static int GetId(this ClaimsPrincipal claimsPrincipal) {
      var subject = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Sub);

      if (subject == null) {
        throw new ArgumentException("Claims principal doesn't have subject claim");
      }

      if (int.TryParse(subject, out var userId)) {
        return userId;
      }

      throw new ArgumentException("Claims principal has invalid subject claim");
    }
  }
}
