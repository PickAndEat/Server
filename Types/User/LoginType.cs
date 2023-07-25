using GraphQL.AspNet.Attributes;

namespace PickAndEat.Types.User {
  [GraphType("UserLogin")]
  public class LoginType {
    public required string AccessToken { get; set; }
  }
}
