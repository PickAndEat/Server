using GraphQL.AspNet.Attributes;

namespace PickAndEat.Types.User {
  [GraphType("UserRegister")]
  public class RegisterType {
    public required int Id { get; set; }
  }
}
