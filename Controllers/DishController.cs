using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using GraphQL.AspNet.Interfaces.Controllers;
using GraphQL.AspNet.ServerExtensions.MultipartRequests.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PickAndEat.Extensions;
using PickAndEat.Models;
using PickAndEat.Types.Dish;

namespace PickAndEat.Controllers {
  public class DishController : GraphController {
    private Database Database { get; }

    public DishController(Database database) {
      Database = database;
    }

    [Authorize]
    [Mutation(typeof(CreateType), TypeExpression = "Type!")]
    public async Task<IGraphActionResult> Create([FromGraphQL("image", TypeExpression = "Type!")] FileUpload imageUpload) {
      if (imageUpload == null) {
        return Error("Missing image", "MISSING_IMAGE");
      }

      var filename = $"{Guid.NewGuid()}.webp";
      var dish = new DishModel { ImageFilename = filename, UserId = User.GetId() };

      using (var imageStream = await imageUpload.OpenFileAsync())
      using (var image = Image.Load(imageStream)) {
        image.Mutate(i => i.Resize(100, 100));
        await image.SaveAsWebpAsync(filename);
      }

      Database.Dishes.Add(dish);
      await Database.SaveChangesAsync();

      return Ok(new CreateType { Id = dish.Id });
    }

    [Authorize]
    [Query(typeof(IEnumerable<ListType>), TypeExpression = "[Type!]!")]
    public async Task<IGraphActionResult> List() {
      var dishes = await Database.Dishes.Where(d => d.UserId == User.GetId()).ToListAsync();

      return Ok(dishes.Select(d => new ListType { Id = d.Id }));
    }
  }
}
