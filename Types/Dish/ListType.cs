using GraphQL.AspNet.Attributes;

namespace PickAndEat.Types.Dish {
  [GraphType("DishList")]
  public class ListType {
    public required int Id { get; set; }
  }
}
