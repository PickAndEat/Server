using GraphQL.AspNet.Attributes;

namespace PickAndEat.Types.Dish {
  [GraphType("DishCreate")]
  public class CreateType {
    public int Id { get; set; }
  }
}
