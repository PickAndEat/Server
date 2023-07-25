// Pick & Eat Server
// Copyright (C) 2023  Louis Matthijssen
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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
